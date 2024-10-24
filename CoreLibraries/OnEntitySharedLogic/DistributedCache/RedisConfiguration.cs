namespace OnEntitySharedLogic.DistributedCache;

public class RedisConfiguration
{
    public string HostName { get; set; } = "localhost";
    public string Port { get; set; } = "6379";
    public string Password { get; set; } = "Admin123!";
    public bool Ssl { get; set; } = false;
}