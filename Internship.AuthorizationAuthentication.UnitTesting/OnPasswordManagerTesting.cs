using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Internship.AuthorizationAuthentication.Api.Infrastructure.Utils;

namespace Internship.AuthorizationAuthentication.UnitTesting;

public class OnPasswordManagerTesting
{
    private readonly IPasswordManager _passwordManager;

    public OnPasswordManagerTesting()
    {
        _passwordManager = new PasswordManager();
    }

    [Fact]
    public void Password_Manager_Returns_False_If_Password_Do_Not_Match()
    {
        var password = _passwordManager.HashPassword("12345");
        Assert.False(_passwordManager.VerifyPassword("1234", password.HashedPassword, password.PasswordSalt));
    }

    [Fact]
    public void Password_Manager_Returns_True_If_Passwords_Match()
    {
        var password = _passwordManager.HashPassword("12555");
        Assert.True(_passwordManager.VerifyPassword("12555",  password.HashedPassword, password.PasswordSalt));
    }
}