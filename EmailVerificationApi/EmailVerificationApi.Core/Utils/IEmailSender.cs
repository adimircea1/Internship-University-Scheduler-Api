using EmailVerification.Library.DataContracts;

namespace EmailVerificationApi.Core.Utils;

public interface IEmailSender
{
    public Task SendCredentialsAsync(UserDataContract data);
    public Task SendGradeDataAsync();
}