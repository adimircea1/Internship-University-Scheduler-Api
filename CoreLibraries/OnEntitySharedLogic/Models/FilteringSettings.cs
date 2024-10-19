namespace OnEntitySharedLogic.Models;

public class FilteringSettings
{
    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public Dictionary<string, string> FilterBy { get; set; } = new Dictionary<string, string>();
}