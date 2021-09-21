using System.Threading.Tasks;
using EmailWorker.Models;

namespace EmailWorker.Service
{
    public interface IEMailSenderService
    {
        Task SendMail(EmailDto emailDto);
    }
}