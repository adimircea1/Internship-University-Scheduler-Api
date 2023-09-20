namespace OnEntitySharedLogic.Models;

public class PaginationSetting
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string OrderBy { get; set; } = string.Empty;
    public OrderByDirection OrderDirection { get; set; }
}