namespace Lab5.Areas.Feed.Models;

public sealed class Tag
{
    public int TagId { get; set; }
    public string TagName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int UsageCount { get; set; }

    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}
