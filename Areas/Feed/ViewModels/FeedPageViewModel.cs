namespace Lab5.Areas.Feed.ViewModels;

public class FeedPageViewModel
{
    public IReadOnlyList<PostCardViewModel> Posts { get; set; } = Array.Empty<PostCardViewModel>();
    public IReadOnlyList<TrendingPondViewModel> TrendingPonds { get; set; } = Array.Empty<TrendingPondViewModel>();
}
