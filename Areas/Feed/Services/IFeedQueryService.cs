using Lab5.Areas.Feed.ViewModels;

namespace Lab5.Areas.Feed.Services;

public interface IFeedQueryService
{
    Task<FeedPageViewModel> GetFeedAsync(CancellationToken cancellationToken = default);
    Task<ProfilePageViewModel?> GetProfileAsync(string username, CancellationToken cancellationToken = default);
    Task<PostDetailsPageViewModel?> GetPostDetailsAsync(long postId, CancellationToken cancellationToken = default);
    Task<PondPageViewModel?> GetPondAsync(string tag, CancellationToken cancellationToken = default);
}
