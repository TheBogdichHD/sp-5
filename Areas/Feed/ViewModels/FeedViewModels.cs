namespace Lab5.Areas.Feed.ViewModels;

public sealed class FeedPageViewModel
{
    public string? CurrentTag { get; init; }
    public IReadOnlyList<PostCardViewModel> Posts { get; init; } = [];
    public IReadOnlyList<TrendingPondItemViewModel> TrendingPonds { get; init; } = [];
}

public sealed class ProfilePageViewModel
{
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public string? Bio { get; init; }
    public int Followers { get; init; }
    public int Following { get; init; }
    public IReadOnlyList<PostCardViewModel> Posts { get; init; } = [];
    public IReadOnlyList<TrendingPondItemViewModel> TrendingPonds { get; init; } = [];
}

public sealed class PostDetailsPageViewModel
{
    public PostCardViewModel Post { get; init; } = new();
    public IReadOnlyList<PostAnswerViewModel> Answers { get; init; } = [];
    public IReadOnlyList<TrendingPondItemViewModel> TrendingPonds { get; init; } = [];
}

public sealed class PostAnswerViewModel
{
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public string ContentWithLinks => HashtagLinkFormatter.WithLinks(Content);

    public string RelativeTime
    {
        get
        {
            var delta = DateTime.UtcNow - CreatedAt;
            if (delta.TotalMinutes < 60)
            {
                return $"{Math.Max(1, (int)delta.TotalMinutes)}m";
            }

            if (delta.TotalHours < 24)
            {
                return $"{(int)delta.TotalHours}h";
            }

            return $"{(int)delta.TotalDays}d";
        }
    }
}

public sealed class PostCardViewModel
{
    public int PostId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string PostType { get; init; } = "text";
    public string? MediaUrl { get; init; }
    public string? MediaType { get; init; }
    public string? AltText { get; init; }
    public string? ThumbnailUrl { get; init; }
    public bool IsEvent { get; init; }
    public DateTime? EventTime { get; init; }
    public string? EventLocation { get; init; }
    public string? EventHostOrg { get; init; }
    public int? EventRsvpCount { get; init; }
    public InteractionSummaryViewModel Interactions { get; init; } = new();
    public IReadOnlyList<string> Tags { get; init; } = [];

    public string ContentWithLinks => HashtagLinkFormatter.WithLinks(Content);

    public string RelativeTime
    {
        get
        {
            var delta = DateTime.UtcNow - CreatedAt;
            if (delta.TotalMinutes < 60)
            {
                return $"{Math.Max(1, (int)delta.TotalMinutes)}m";
            }

            if (delta.TotalHours < 24)
            {
                return $"{(int)delta.TotalHours}h";
            }

            return $"{(int)delta.TotalDays}d";
        }
    }
}

public sealed class InteractionSummaryViewModel
{
    public int Comments { get; init; }
    public int Reribbs { get; init; }
    public int Likes { get; init; }
    public int Rsvps { get; init; }
}

public sealed class TrendingPondItemViewModel
{
    public string TagName { get; init; } = string.Empty;
    public int RecentPosts { get; init; }
}
