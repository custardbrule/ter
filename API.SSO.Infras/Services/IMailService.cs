using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Services
{
    public enum EMailTempalte
    {
        Register = 1,
    }

    public interface IMailService
    {
        Task SendMail(string[] addresses, string subject, EMailTempalte template, object value);
    }
}
