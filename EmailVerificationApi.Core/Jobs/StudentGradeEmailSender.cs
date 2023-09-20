using EmailVerificationApi.Core.Utils;
using Quartz;

namespace EmailVerificationApi.Core.Jobs;

public class StudentGradeEmailSender : IJob
{
    private readonly IEmailSender _emailSender;

    public StudentGradeEmailSender(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _emailSender.SendGradeDataAsync();
    }
}