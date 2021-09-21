using System.Threading.Tasks;
using EmailWorker.Models;

namespace EmailWorker.Service
{
    public interface IEMailSenderService
    {
        Task SendMailAsync(EmailDto emailDto);
    }
}