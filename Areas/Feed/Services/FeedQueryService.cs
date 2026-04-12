using Lab5.Areas.Feed.Data;
using Lab5.Areas.Feed.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Areas.Feed.Services;

public sealed class FeedQueryService(GreenswampContext context) : IFeedQueryService
{
    public async Task<FeedPageViewModel> GetFeedAsync(string? tag, CancellationToken cancellationToken = default)
    {
        var posts = await BuildPostCardsQuery(tag)
            .OrderByDescending(x => x.CreatedAt)
            .Take(100)
            .ToListAsync(cancellationToken);

        return new FeedPageViewModel
        {
            CurrentTag = string.IsNullOrWhiteSpace(tag) ? null : tag,
            Posts = posts,
            TrendingPonds = await GetTrendingPondsAsync(cancellationToken)
        };
    }

    public async Task<ProfilePageViewModel?> GetProfileAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username && x.IsActive, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var posts = await BuildPostCardsQuery(null)
            .Where(x => x.Username == username)
            .OrderByDescending(x => x.CreatedAt)
            .Take(100)
            .ToListAsync(cancellationToken);

        return new ProfilePageViewModel
        {
            Username = user.Username,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            Followers = 58,
            Following = 120,
            Posts = posts,
            TrendingPonds = await GetTrendingPondsAsync(cancellationToken)
        };
    }

    public async Task<PostDetailsPageViewModel?> GetPostAsync(int postId, CancellationToken cancellationToken = default)
    {
        var post = await BuildPostCardsQuery(null)
            .FirstOrDefaultAsync(x => x.PostId == postId, cancellationToken);

        if (post is null)
        {
            return null;
        }

        var answers = await context.Interactions
            .AsNoTracking()
            .Where(x => x.PostId == postId && x.InteractionType == "comment" && x.User.IsActive)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new PostAnswerViewModel
            {
                Username = x.User.Username,
                DisplayName = x.User.DisplayName,
                AvatarUrl = x.User.AvatarUrl,
                Content = x.Content ?? string.Empty,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new PostDetailsPageViewModel
        {
            Post = post,
            Answers = answers,
            TrendingPonds = await GetTrendingPondsAsync(cancellationToken)
        };
    }

    private IQueryable<PostCardViewModel> BuildPostCardsQuery(string? tag)
    {
        var posts = context.Posts
            .AsNoTracking()
            .Where(x => x.User.IsActive);

        if (!string.IsNullOrWhiteSpace(tag))
        {
            posts = posts.Where(x => x.PostTags.Any(pt => pt.Tag.TagName == tag));
        }

        return posts.Select(x => new PostCardViewModel
        {
            PostId = x.PostId,
            Username = x.User.Username,
            DisplayName = x.User.DisplayName,
            AvatarUrl = x.User.AvatarUrl,
            Content = x.Content,
            CreatedAt = x.CreatedAt,
            PostType = x.PostType,
            MediaUrl = x.MediaUrl,
            MediaType = x.MediaType,
            AltText = x.AltText,
            ThumbnailUrl = x.ThumbnailUrl,
            IsEvent = x.Event != null,
            EventTime = x.Event != null ? x.Event.EventTime : null,
            EventLocation = x.Event != null ? x.Event.Location : null,
            EventHostOrg = x.Event != null ? x.Event.HostOrg : null,
            EventRsvpCount = x.Event != null ? x.Event.RsvpCount : null,
            Tags = x.PostTags
                .OrderBy(pt => pt.Tag.TagName)
                .Select(pt => pt.Tag.TagName)
                .ToList(),
            Interactions = new InteractionSummaryViewModel
            {
                Comments = x.Interactions.Count(i => i.InteractionType == "comment"),
                Reribbs = x.Interactions.Count(i => i.InteractionType == "reribb"),
                Likes = x.Interactions.Count(i => i.InteractionType == "like"),
                Rsvps = x.Interactions.Count(i => i.InteractionType == "rsvp")
            }
        });
    }

    private async Task<IReadOnlyList<TrendingPondItemViewModel>> GetTrendingPondsAsync(CancellationToken cancellationToken)
    {
        var since = DateTime.UtcNow.AddDays(-1);

        var recent = await context.PostTags
            .AsNoTracking()
            .Where(x => x.Post.CreatedAt >= since)
            .GroupBy(x => x.Tag.TagName)
            .Select(g => new TrendingPondItemViewModel
            {
                TagName = g.Key,
                RecentPosts = g.Count()
            })
            .OrderByDescending(x => x.RecentPosts)
            .Take(10)
            .ToListAsync(cancellationToken);

        if (recent.Count > 0)
        {
            return recent;
        }

        return await context.Tags
            .AsNoTracking()
            .OrderByDescending(x => x.UsageCount)
            .ThenBy(x => x.TagName)
            .Select(x => new TrendingPondItemViewModel
            {
                TagName = x.TagName,
                RecentPosts = x.UsageCount
            })
            .Take(10)
            .ToListAsync(cancellationToken);
    }
}
