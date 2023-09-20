using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Models.Input;

namespace Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;

public interface IUserAuthenticationService
{
    public Task<string> RegisterStudentUserAsync(int studentRegisterRequestId);
    public Task<string> RegisterProfessorUserAsync(int professorRegisterRequestId);
    public Task<TokenModel> LoginUserAsync(LoginRequest request);
    public Task<TokenModel> RefreshTokenAsync(RefreshRequest request);
    public Task LogoutUserAsync(string? userId);
}