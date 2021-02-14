using BackendComfeco.DTOs.Email;

using System.Threading.Tasks;

namespace BackendComfeco.Helpers
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
