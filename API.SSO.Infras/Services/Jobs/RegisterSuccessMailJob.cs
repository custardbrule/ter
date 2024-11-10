using Core.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Services.Jobs
{
    public class SendRegisterEmailJobModel
    {
        public string[] Emails { get; set; } 
        public string Subject { get; set; } 
        public EMailTempalte Template { get; set; } 
        public object Data { get; set; }
    } 

    public class RegisterSuccessMailJob : IJob
    {
        private readonly IMailService _mailService;

        public RegisterSuccessMailJob(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var model = context.JobDetail.JobDataMap.GetString("Model")!.Deserialize<SendRegisterEmailJobModel>()!;
            await _mailService.SendMail(model.Emails, model.Subject, model.Template, model.Data);
        }
    }
}
