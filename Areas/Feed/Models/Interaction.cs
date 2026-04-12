namespace Lab5.Areas.Feed.Models;

public sealed class Interaction
{
    public int InteractionId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Content { get; set; }

    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;
}
