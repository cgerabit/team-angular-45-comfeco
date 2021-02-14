using TeamAngular45Backend.DTOs.Email;

using System.Threading.Tasks;

namespace TeamAngular45Backend.Helpers
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
