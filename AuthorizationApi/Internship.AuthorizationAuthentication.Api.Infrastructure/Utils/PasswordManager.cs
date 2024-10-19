using System.Security.Cryptography;
using System.Text;
using Internship.AuthorizationAuthentication.Api.Core.Models.Utils;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Infrastructure.Utils;

[Registration(Type = RegistrationKind.Singleton)]
public class PasswordManager : IPasswordManager
{
    private const int SaltSize = 32;
    
    private static string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[SaltSize];
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }
    
    public PasswordData HashPassword(string passwordToHash)
    {
        var salt = GenerateSalt();
        var combinedPassword = string.Concat(passwordToHash, salt);
        var byteValue = Encoding.UTF8.GetBytes(passwordToHash);
        var byteHash = SHA256.HashData(byteValue);
        var hashedPassword = Convert.ToBase64String(byteHash);
        return new PasswordData
        {
            HashedPassword = hashedPassword,
            PasswordSalt = salt
        };
    }

    private string HashPassword(string passwordToHash, string passwordSalt)
    {
        var combinedPassword = string.Concat(passwordToHash, passwordSalt);
        var byteValue = Encoding.UTF8.GetBytes(passwordToHash);
        var byteHash = SHA256.HashData(byteValue);
        return Convert.ToBase64String(byteHash);
    }
    

    public bool VerifyPassword(string passwordToVerify, string userHashedPassword, string userPasswordSalt)
    {
        var hashOfEnteredPassword = HashPassword(passwordToVerify, userPasswordSalt);
        return string.Equals(hashOfEnteredPassword, userHashedPassword);
    }
    
    public string GenerateTemporaryPassword(int length)
    {
        var random = new Random();
        
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        var tempPassword = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var index = random.Next(validChars.Length);
            tempPassword.Append(validChars[index]);
        }

        return tempPassword.ToString();
    }
}