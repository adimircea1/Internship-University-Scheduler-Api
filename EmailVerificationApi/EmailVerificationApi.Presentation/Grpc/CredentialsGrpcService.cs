using EmailVerification.Library.DataContracts;
using EmailVerification.Library.GrpcServiceInterfaces;
using EmailVerificationApi.Core.Utils;
using Grpc.Core;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace EmailVerificationApi.Presentation.Grpc;

[Registration(Type = RegistrationKind.Scoped)]
public class CredentialsGrpcService : ICredentialsGrpcService
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<CredentialsGrpcService> _logger;

    public CredentialsGrpcService(
        IEmailSender emailSender,
        ILogger<CredentialsGrpcService> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public async ValueTask SendUserCredentialsAsync(UserDataContract userData)
    {
        try
        {
            await _emailSender.SendCredentialsAsync(userData);
            _logger.LogInformation($"{DateTime.Now} ---> Successfully sent email with user credentials!");
        }
        catch (Exception ex)
        {
            var status = new Status(StatusCode.Internal, ex.Message);
            throw new RpcException(status);
        }
    }
}