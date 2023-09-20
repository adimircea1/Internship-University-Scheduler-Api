namespace OnEntitySharedLogic.Models;

public class FilterOrderSettings
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string OrderBy { get; set; } = string.Empty;
    public Dictionary<string, string> FilterBy { get; set; } = new();
    public OrderByDirection OrderDirection { get; set; }
}