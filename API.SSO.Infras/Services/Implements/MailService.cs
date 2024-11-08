using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Services.Implements
{
    public class MailService : IMailService
    {
        public Task SendMail(string[] addresses, string subject, EMailTempalte template, object value)
        {
            Console.WriteLine("Mail Sended");
            return Task.CompletedTask;
        }
    }
}
