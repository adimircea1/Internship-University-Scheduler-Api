namespace StudentExamination.Api.Core.Utils.Configuration;

public class AuthenticationConfiguration
{
    public string SecretToken { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}