using System.Threading.Tasks;

namespace Glare.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}