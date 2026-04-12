namespace Lab5.Areas.Feed.Models;

public sealed class TrendingPond
{
    public int TagId { get; set; }
    public string TagName { get; set; } = string.Empty;
    public int RecentPosts { get; set; }
}
