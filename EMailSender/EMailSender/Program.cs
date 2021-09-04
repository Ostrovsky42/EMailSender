using System.Collections.Generic;
using System.Net.Mail;
using Serilog;

namespace EMailSender
{
    class Program
    {
        static void Main(string[] args)
        {

            var emailSenderService = new EMailSenderService();
            var listMail = new List<MailAddress>();

            //listMail.Add(new("tootoo9723@gmail.com"));
            listMail.Add(new("Zhekul.90@gmail.com"));

            emailSenderService.SendMail("Test", "Dear Student", listMail);

            Log.Information("dispoce log");
        }
    }
}