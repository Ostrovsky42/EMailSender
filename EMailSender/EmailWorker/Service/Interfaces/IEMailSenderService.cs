using EmailWorker.Models;

namespace EmailWorker.Service
{
    public interface IEMailSenderService
    {
        void SendMail(EmailDto emailDto);
    }
}