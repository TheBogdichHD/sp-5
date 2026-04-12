namespace Lab5.Areas.Feed.Models;

public sealed class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
}
