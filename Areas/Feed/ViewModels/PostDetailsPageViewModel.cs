namespace Lab5.Areas.Feed.ViewModels;

public class PostDetailsPageViewModel
{
    public PostCardViewModel Post { get; set; } = new();
    public IReadOnlyList<PostCardViewModel> Replies { get; set; } = Array.Empty<PostCardViewModel>();
    public IReadOnlyList<TrendingPondViewModel> TrendingPonds { get; set; } = Array.Empty<TrendingPondViewModel>();
}
