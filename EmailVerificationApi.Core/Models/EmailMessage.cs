namespace EmailVerificationApi.Core.Models;

public class EmailMessage
{
    public string From { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<string> To { get; set; } = new();
    public bool IsBodyHtml { get; set; }
}