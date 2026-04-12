namespace Lab5.Areas.Feed.Models;

public sealed class PostTag
{
    public int PostId { get; set; }
    public int TagId { get; set; }

    public Post Post { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
