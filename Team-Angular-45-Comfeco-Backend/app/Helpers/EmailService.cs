using BackendComfeco.DTOs.Email;
using BackendComfeco.Shared.Settings;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Options;

using MimeKit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackendComfeco.Helpers
{
    public class EmailService : IEmailService, IDisposable
    {
        private readonly MailSettings _mailSettings;

        private List<MailRequest> mailRequests = new List<MailRequest>();

        private readonly SemaphoreSlim recollectionSemaphore = new SemaphoreSlim(1);

        // Only 1 email at same time
        private readonly SemaphoreSlim emailSemaphore = new SemaphoreSlim(1);

        // we check for new emails every 3 seconds 
        readonly System.Timers.Timer EmailTimer
              = new System.Timers.Timer(TimeSpan.FromSeconds(3).TotalMilliseconds);

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;

            EmailTimer.Elapsed += EmailTimer_Elapsed;
            EmailTimer.Start();
        }

        private async void EmailTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<MailRequest> processingRequest;

            try
            {
                // Evitar condicion de carrera
                await recollectionSemaphore.WaitAsync();
                processingRequest = new List<MailRequest>(mailRequests);
                mailRequests = new List<MailRequest>();
            }
            finally
            {
                recollectionSemaphore.Release();
            }
            if (processingRequest==null || processingRequest.Count == 0)
            {
                return;
            }

            using var smtp = new SmtpClient();
            try
            {
              
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            }
            catch (Exception)
            {

                // TODO: 
                // add logger
                //error has been ocurred connecting to the server we return emails to the list

                try
                {
                    await recollectionSemaphore.WaitAsync();
                    mailRequests.AddRange(processingRequest);
                }
                finally
                {
                    recollectionSemaphore.Release();
                }
            }

            if( !smtp.IsConnected || !smtp.IsAuthenticated)
            {
                return;
            }

           var emailSendingTasks =  processingRequest.Select(async emailReq => {

               try
               {

                   var email = new MimeMessage
                   {
                       Sender = MailboxAddress.Parse(_mailSettings.Mail)
                   };
                   email.To.Add(MailboxAddress.Parse(emailReq.ToEmail));
                   email.Subject = emailReq.Subject;

                   var builder = new BodyBuilder
                   {
                       HtmlBody = emailReq.Body
                   };
                   email.Body = builder.ToMessageBody();
                   await emailSemaphore.WaitAsync();
                   await smtp.SendAsync(email);
               }
               finally
               {
                   emailSemaphore.Release();
               }
            });


            await Task.WhenAll(emailSendingTasks);
          
            smtp.Disconnect(true);

        }



        public async Task SendEmailAsync(MailRequest mailRequest)
        {


            try
            {
                // avoid race condition
                await recollectionSemaphore.WaitAsync();
                mailRequests.Add(mailRequest);
            }
            finally
            {
                recollectionSemaphore.Release();
            }


        }

        public void Dispose()
        {
            EmailTimer?.Dispose();
        }
    }
}
