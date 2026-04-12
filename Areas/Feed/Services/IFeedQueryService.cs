using Lab5.Areas.Feed.ViewModels;

namespace Lab5.Areas.Feed.Services;

public interface IFeedQueryService
{
    Task<FeedPageViewModel> GetFeedAsync(string? tag, CancellationToken cancellationToken = default);
    Task<ProfilePageViewModel?> GetProfileAsync(string username, CancellationToken cancellationToken = default);
    Task<PostDetailsPageViewModel?> GetPostAsync(int postId, CancellationToken cancellationToken = default);
}
