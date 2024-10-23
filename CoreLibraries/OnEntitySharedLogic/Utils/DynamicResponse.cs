namespace OnEntitySharedLogic.Utils;

public class DynamicResponse
{
    public DynamicResponse(int statusCode)
    {
        StatusCode = statusCode;
    }
    
    public DynamicResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public DynamicResponse(int statusCode, string message, dynamic payload)
    {
        StatusCode = statusCode;
        Message = message;
        Payload = payload;
    }
    
    public int StatusCode { get; set; }
    public string? Message { get; set; } 
    public dynamic? Payload { get; set; }
}