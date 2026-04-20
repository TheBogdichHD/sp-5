namespace Lab5.Areas.Feed.Models;

public class PostTag
{
    public long PostId { get; set; }
    public long TagId { get; set; }

    public Post Post { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
