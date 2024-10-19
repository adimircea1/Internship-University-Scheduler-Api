namespace EmailVerificationApi.Core.Models;

public class SmtpClientConfiguration
{
    public string From { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}