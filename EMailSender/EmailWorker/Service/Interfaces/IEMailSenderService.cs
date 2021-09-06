using EmailWorker.Models;
using EmailWorker.Settings;
using Microsoft.Extensions.Options;

namespace EmailWorker.Service
{
    public interface IEMailSenderService
    {
        void SendMail(EmailDto emailDto, IOptions<ConnSettings> options);
    }
}