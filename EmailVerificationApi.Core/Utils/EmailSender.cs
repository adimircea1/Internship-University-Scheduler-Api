using System.Net;
using System.Net.Mail;
using System.Text;
using EmailVerification.Library.DataContracts;
using EmailVerificationApi.Core.Models;
using Internship.UniversityScheduler.Library.SharedModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.RabbitMq;
using OnEntitySharedLogic.Utils;

namespace EmailVerificationApi.Core.Utils;

[Registration(Type = RegistrationKind.Scoped)]
public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly SmtpClientConfiguration _smtpClient;
    private readonly IMessageConsumer _messageConsumer;
    private readonly IConfiguration _configuration;

    public EmailSender(
        ILogger<EmailSender> logger,
        SmtpClientConfiguration smtpClient,
        IMessageConsumer messageConsumer,
        IConfiguration configuration)
    {
        _logger = logger;
        _smtpClient = smtpClient;
        _messageConsumer = messageConsumer;
        _configuration = configuration;
    }

    public async Task SendCredentialsAsync(UserDataContract data)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpClient.From),
            Subject = "Arasaka university user credentials",
            IsBodyHtml = true,
            Body = $"Username:{data.UserName} TemporaryPassword:{data.TemporaryPassword}"
        };

        mailMessage.To.Add(new MailAddress(data.ReceiverEmail));

        try
        {
            await GetClient().SendMailAsync(mailMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{DateTime.Now} ---> {e.Message}");
            throw;
        }
    }

    public Task SendGradeDataAsync()
    {
        var queue = _configuration.GetSection("RabbitMqQueues")["StudentGradeDataDistributionQueue"];
        var client = GetClient();

        if (string.IsNullOrEmpty(queue))
        {
            throw new EntityNotFoundException("Could not find specified queue!");
        }

        _messageConsumer.ConsumeMessage(queue, eventArgs =>
        {
            var messageToByteArray = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(messageToByteArray);
            var studentGradesEmail = JsonConvert.DeserializeObject<StudentGradesEmail>(message)!;

            var emailBodyBuilder = new StringBuilder();

            foreach (var grade in studentGradesEmail.Grades)
            {
                emailBodyBuilder.Append($"Grade Value: {grade.GradeValue}<br/>");
                emailBodyBuilder.Append($"Course Domain: {grade.CourseDomain}<br/>");
                emailBodyBuilder.Append($"Date of Grade: {grade.DateOfGrade.ToString("g")}<br/><br/>");
            }
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpClient.From),
                Subject = "Arasaka university weekly student grades report",
                IsBodyHtml = true,
                Body = emailBodyBuilder.ToString()
            };
            
            mailMessage.To.Add(new MailAddress(studentGradesEmail.StudentEmail));

            Task.Run(async () =>
            {
                try
                {
                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"{DateTime.Now} ---> Successfully sent email to student!");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{DateTime.Now} ---> {e.Message}");
                    throw;
                }
            });
        });

        return Task.CompletedTask;
    }

    private SmtpClient GetClient()
    {
        var client = new SmtpClient
        {
            Host = _smtpClient.Host,
            Port = _smtpClient.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(_smtpClient.UserName, _smtpClient.Password),
        };

        return client;
    }
}