namespace Lab5.Areas.Feed.ViewModels;

public class ProfilePageViewModel
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<PostCardViewModel> Posts { get; set; } = Array.Empty<PostCardViewModel>();
    public IReadOnlyList<TrendingPondViewModel> TrendingPonds { get; set; } = Array.Empty<TrendingPondViewModel>();
}
