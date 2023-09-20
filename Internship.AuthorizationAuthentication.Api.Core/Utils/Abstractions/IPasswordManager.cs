using Internship.AuthorizationAuthentication.Api.Core.Models.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

public interface IPasswordManager
{
    public PasswordData HashPassword(string passwordToHash);
    public bool VerifyPassword(string passwordToVerify, string hashedUserPassword, string userPasswordSalt);
    public string GenerateTemporaryPassword(int length);
}