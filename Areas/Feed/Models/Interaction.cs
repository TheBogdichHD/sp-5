namespace Lab5.Areas.Feed.Models;

public class Interaction
{
    public long InteractionId { get; set; }
    public long UserId { get; set; }
    public long PostId { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Content { get; set; }

    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;
}
