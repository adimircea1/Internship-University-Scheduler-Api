using Internship.AuthorizationAuthentication.Api.Core.Models.Input;

namespace Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;

public interface IUserAuthenticationService
{
    public Task<string> RegisterStudentUserAsync(int studentRegisterRequestId);
    public Task<string> RegisterProfessorUserAsync(int professorRegisterRequestId);
    public Task<string> LoginUserAsync(LoginRequest request);
    public Task InvalidateUserTokenAsync(int userId);
    public Task LogoutUserAsync(string? userId);
}