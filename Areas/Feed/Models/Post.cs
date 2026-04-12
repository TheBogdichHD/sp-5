namespace Lab5.Areas.Feed.Models;

public sealed class Post
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string PostType { get; set; } = "text";
    public string? MediaUrl { get; set; }
    public string? MediaType { get; set; }
    public string? AltText { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ParentPostId { get; set; }

    public User User { get; set; } = null!;
    public Post? ParentPost { get; set; }
    public ICollection<Post> Replies { get; set; } = new List<Post>();
    public Event? Event { get; set; }
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
}
