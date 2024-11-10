using API.SSO.Infras.Services.Jobs;
using API.SSO.Infras.Services;
using MediatR;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;

namespace API.SSO.Infras.Features.UserManagement.Notifications
{
    public record RegisterEmailNotify(string Email) : INotification;

    public class RegisterEmailNotifyHandler : INotificationHandler<RegisterEmailNotify>
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public RegisterEmailNotifyHandler(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task Handle(RegisterEmailNotify notification, CancellationToken cancellationToken)
        {
            var jobKey = Guid.NewGuid().ToString();

            var jobData = new SendRegisterEmailJobModel
            {
                Emails = [notification.Email],
                Subject = "Register Success",
                Template = EMailTempalte.Register,
                Data = new { }
            };

            var job = JobBuilder
                .Create<RegisterSuccessMailJob>()
                .WithIdentity(jobKey, nameof(RegisterSuccessMailJob))
                .PersistJobDataAfterExecution()
                .UsingJobData("Model", jobData.Serialize())
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity(jobKey, nameof(RegisterSuccessMailJob))
                .StartNow()
                .ForJob(job.Key)
                .Build();

            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }
    }
}
