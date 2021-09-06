using EMailSenderService.Models;

namespace EMailSenderService.Service
{
    public interface IEMailSenderService
    {
        void SendMail(EmailDto emailDto);
    }
}