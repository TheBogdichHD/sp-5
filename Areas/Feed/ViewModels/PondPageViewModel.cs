namespace Lab5.Areas.Feed.ViewModels;

public class PondPageViewModel
{
    public string TagName { get; set; } = string.Empty;
    public IReadOnlyList<PostCardViewModel> Posts { get; set; } = Array.Empty<PostCardViewModel>();
    public IReadOnlyList<TrendingPondViewModel> TrendingPonds { get; set; } = Array.Empty<TrendingPondViewModel>();
}
