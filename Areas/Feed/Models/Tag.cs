namespace Lab5.Areas.Feed.Models;

public class Tag
{
    public long TagId { get; set; }
    public string TagName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public long UsageCount { get; set; }

    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}
