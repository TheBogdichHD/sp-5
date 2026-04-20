namespace Lab5.Areas.Feed.ViewModels;

public class PostCardViewModel
{
    public long PostId { get; set; }
    public long? ParentPostId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public string AuthorDisplayName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }
    public string ContentHtml { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedAgo { get; set; } = string.Empty;

    public string PostType { get; set; } = "text";
    public string? MediaUrl { get; set; }
    public string? MediaType { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }

    public bool IsEvent => EventTime.HasValue;
    public DateTime? EventTime { get; set; }
    public string? EventLocation { get; set; }
    public string? EventHostOrg { get; set; }
    public long? EventRsvpCount { get; set; }

    public int InteractionsCount { get; set; }
    public int AnswersCount { get; set; }
    public int ReribbsCount { get; set; }

    public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();
}
