using Lab5.Areas.Feed.Database;
using Lab5.Areas.Feed.Models;
using Lab5.Areas.Feed.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Areas.Feed.Services;

public class FeedQueryService(GreenswampContext db, IHashtagFormatter hashtagFormatter) : IFeedQueryService
{
    public async Task<FeedPageViewModel> GetFeedAsync(CancellationToken cancellationToken = default)
    {
        var posts = await LoadPostsBaseQuery()
            .Where(x => x.ParentPostId == null)
            .OrderByDescending(x => x.CreatedAt)
            .Take(100)
            .ToListAsync(cancellationToken);

        return new FeedPageViewModel
        {
            Posts = posts.Select(MapPost).ToList(),
            TrendingPonds = await GetTrendingPondsAsync(cancellationToken)
        };
    }

    public async Task<ProfilePageViewModel?> GetProfileAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username && x.IsActive, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var posts = await LoadPostsBaseQuery()
            .Where(x => x.UserId == user.UserId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return new ProfilePageViewModel
        {
            Username = user.Username,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            CreatedAt = user.CreatedAt,
            Posts = posts.Select(MapPost).ToList(),
            TrendingPonds = await GetTrendingPondsAsync(cancellationToken)
        };
    }

    public async Task<PostDetailsPageViewModel?> GetPostDetailsAsync(long postId, CancellationToken cancellationToken = default)
    {
        var post = await LoadPostsBaseQuery()
            .FirstOrDefaultAsync(x => x.PostId == postId, cancellationToken);

        if (post is null)
        {
            return null;
        }

        var candidateReplies = await LoadPostsBaseQuery()
            .Where(x => x.ParentPostId != null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        var repliesByParent = candidateReplies
            .GroupBy(x => x.ParentPostId!.Value)
            .ToDictionary(x => x.Key, x => x.ToList());

        var replies = new List<Post>();
        var visited = new HashSet<long>();
        var queue = new Queue<long>();
        queue.Enqueue(postId);

        while (queue.Count > 0)
        {
            var parentId = queue.Dequeue();
            if (!repliesByParent.TryGetValue(parentId, out var children))
            {
                continue;
            }

            foreach (var child in children)
            {
                if (!visited.Add(child.PostId))
                {
                    continue;
                }

                replies.Add(child);
                queue.Enqueue(child.PostId);
            }
        }

        return new PostDetailsPageViewModel
        {
            Post = MapPost(post),
            Replies = replies.Select(MapPost).ToList(),
            TrendingPonds = await GetTrendingPondsAsync(cancellationToken)
        };
    }

    public async Task<PondPageViewModel?> GetPondAsync(string tag, CancellationToken cancellationToken = default)
    {
        var normalizedTag = tag.Trim();
        if (string.IsNullOrWhiteSpace(normalizedTag))
        {
            return null;
        }

        var tagEntity = await db.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TagName == normalizedTag, cancellationToken);

        if (tagEntity is null)
        {
            return null;
        }

        var posts = await LoadPostsBaseQuery()
            .Where(x => x.ParentPostId == null)
            .Where(x => x.PostTags.Any(pt => pt.Tag.TagName == normalizedTag))
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return new PondPageViewModel
        {
            TagName = tagEntity.TagName,
            Posts = posts.Select(MapPost).ToList(),
            TrendingPonds = await GetTrendingPondsAsync(cancellationToken)
        };
    }

    private IQueryable<Post> LoadPostsBaseQuery()
    {
        return db.Posts
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Event)
            .Include(x => x.Interactions)
            .Include(x => x.PostTags)
                .ThenInclude(x => x.Tag);
    }

    private async Task<IReadOnlyList<TrendingPondViewModel>> GetTrendingPondsAsync(CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddDays(-1);
        return await db.PostTags
            .AsNoTracking()
            .Where(x => x.Post.CreatedAt >= cutoff)
            .Where(x => x.Post.ParentPostId == null)
            .GroupBy(x => x.Tag.TagName)
            .OrderByDescending(x => x.Count())
            .Take(10)
            .Select(x => new TrendingPondViewModel
            {
                TagName = x.Key,
                RecentPosts = x.LongCount()
            })
            .ToListAsync(cancellationToken);
    }

    private PostCardViewModel MapPost(Post post)
    {
        return new PostCardViewModel
        {
            PostId = post.PostId,
            ParentPostId = post.ParentPostId,
            AuthorUsername = post.User.Username,
            AuthorDisplayName = post.User.DisplayName,
            AuthorAvatarUrl = post.User.AvatarUrl,
            ContentHtml = hashtagFormatter.Format(post.Content),
            CreatedAt = post.CreatedAt,
            CreatedAgo = ToTimeAgo(post.CreatedAt),
            PostType = post.PostType,
            MediaUrl = post.MediaUrl,
            MediaType = post.MediaType,
            ThumbnailUrl = post.ThumbnailUrl,
            AltText = post.AltText,
            EventTime = post.Event?.EventTime,
            EventLocation = post.Event?.Location,
            EventHostOrg = post.Event?.HostOrg,
            EventRsvpCount = post.Event?.RsvpCount,
            InteractionsCount = post.Interactions.Count,
            AnswersCount = post.Interactions.Count(x => x.InteractionType == "comment"),
            ReribbsCount = post.Interactions.Count(x => x.InteractionType == "reribb"),
            Tags = post.PostTags.Select(x => x.Tag.TagName).Distinct().OrderBy(x => x).ToList()
        };
    }

    private static string ToTimeAgo(DateTime createdAt)
    {
        var delta = DateTime.UtcNow - createdAt;

        if (delta.TotalMinutes < 1)
        {
            return "now";
        }

        if (delta.TotalHours < 1)
        {
            return $"{Math.Max(1, (int)delta.TotalMinutes)}m";
        }

        if (delta.TotalDays < 1)
        {
            return $"{Math.Max(1, (int)delta.TotalHours)}h";
        }

        return $"{Math.Max(1, (int)delta.TotalDays)}d";
    }
}
