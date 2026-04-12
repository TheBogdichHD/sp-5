using Lab5.Areas.Feed.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Areas.Feed.Data;

public static class FeedDatabaseInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<GreenswampContext>();

        await context.Database.EnsureCreatedAsync();
        await SeedIfEmptyAsync(context);
    }

    private static async Task SeedIfEmptyAsync(GreenswampContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;

        var users = new[]
        {
            new User
            {
                Username = "frogger_vig",
                DisplayName = "Viggo The Frogger",
                AvatarUrl = "https://i.pravatar.cc/100?u=frogger_vig@pravatar.com",
                Bio = "Digital artist, coder, and amphibian optimist.",
                CreatedAt = now.AddMonths(-8),
                IsActive = true
            },
            new User
            {
                Username = "dorm23_frogs",
                DisplayName = "FroggyFriends",
                AvatarUrl = "https://i.pravatar.cc/100?u=dorm23_frogs@pravatar.com",
                Bio = "Study groups and late-night tea.",
                CreatedAt = now.AddMonths(-10),
                IsActive = true
            },
            new User
            {
                Username = "mycelium_maddy",
                DisplayName = "FungiFanatic",
                AvatarUrl = "https://i.pravatar.cc/100?u=shroom_lady@pravatar.com",
                Bio = "Mushroom hunter and campus explorer.",
                CreatedAt = now.AddMonths(-6),
                IsActive = true
            },
            new User
            {
                Username = "ptst_toad",
                DisplayName = "Princess Toadstool",
                AvatarUrl = "https://i.pravatar.cc/100?u=ptst_toad@pravatar.com",
                Bio = "Royal swamp updates and umbrella weather.",
                CreatedAt = now.AddMonths(-5),
                IsActive = true
            },
            new User
            {
                Username = "lostnfound",
                DisplayName = "SwampFinder",
                AvatarUrl = "https://i.pravatar.cc/100?u=swamp_finder@pravatar.com",
                Bio = "Lost and found around the ponds.",
                CreatedAt = now.AddMonths(-4),
                IsActive = true
            },
            new User
            {
                Username = "actual_swamp_monster",
                DisplayName = "MossBeard99",
                AvatarUrl = "https://i.pravatar.cc/100?u=swamp_cryptid@pravatar.com",
                Bio = "Cryptid watcher since forever.",
                CreatedAt = now.AddMonths(-7),
                IsActive = true
            },
            new User
            {
                Username = "biology_buff",
                DisplayName = "BioBuddy",
                AvatarUrl = "https://i.pravatar.cc/100?u=biology_buff@pravatar.com",
                Bio = "Biology notes and study help.",
                CreatedAt = now.AddMonths(-3),
                IsActive = true
            },
            new User
            {
                Username = "chem_frog",
                DisplayName = "ChemFrog",
                AvatarUrl = "https://i.pravatar.cc/100?u=chem_frog@pravatar.com",
                Bio = "Need help with biochem always.",
                CreatedAt = now.AddMonths(-3),
                IsActive = true
            },
            new User
            {
                Username = "prcs_toad",
                DisplayName = "ToadPrincess",
                AvatarUrl = "https://i.pravatar.cc/100?u=prcs_toad@pravatar.com",
                Bio = "Fan of pixel frogs.",
                CreatedAt = now.AddMonths(-3),
                IsActive = true
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var tags = new[]
        {
            new Tag { TagName = "StudySwamp", UsageCount = 0, CreatedAt = now },
            new Tag { TagName = "TechMagic", UsageCount = 0, CreatedAt = now },
            new Tag { TagName = "PixelFrog", UsageCount = 0, CreatedAt = now },
            new Tag { TagName = "SwampLife", UsageCount = 0, CreatedAt = now },
            new Tag { TagName = "CampusLife", UsageCount = 0, CreatedAt = now },
            new Tag { TagName = "SwampLostAndFound", UsageCount = 0, CreatedAt = now },
            new Tag { TagName = "ShroomBuddies", UsageCount = 0, CreatedAt = now },
            new Tag { TagName = "SwampMysteries", UsageCount = 0, CreatedAt = now }
        };

        await context.Tags.AddRangeAsync(tags);
        await context.SaveChangesAsync();

        var posts = new[]
        {
            new Post
            {
                UserId = users[1].UserId,
                Content = "Anyone want to form a study group for the biology midterm? Meeting at the lilypad cafe tomorrow 3PM! #StudySwamp",
                PostType = "text",
                CreatedAt = now.AddMinutes(-29)
            },
            new Post
            {
                UserId = users[4].UserId,
                Content = "LOST: Silver water bottle with algae sticker near Turtle Pond. Reward in bug cookies! #SwampLostAndFound",
                PostType = "text",
                CreatedAt = now.AddMinutes(-45)
            },
            new Post
            {
                UserId = users[0].UserId,
                Content = "When the code compiles on the first try #TechMagic #PixelFrog",
                PostType = "video",
                MediaType = "video",
                MediaUrl = "/media/Standard_Mode_a_looed_gif_of_a_pixel_frog_flyi.mp4",
                ThumbnailUrl = "/images/green-toad-logo.svg",
                AltText = "Pixel frog animation",
                CreatedAt = now.AddHours(-2)
            },
            new Post
            {
                UserId = users[5].UserId,
                Content = "Found weird footprints by the old dock! Either a new cryptid friend or Dave forgot his waders again. #SwampMysteries",
                PostType = "text",
                CreatedAt = now.AddHours(-1)
            },
            new Post
            {
                UserId = users[2].UserId,
                Content = "Just found a glowing mushroom in the North Bog! PSA: Do NOT lick it. #ShroomBuddies",
                PostType = "text",
                CreatedAt = now.AddMinutes(-17)
            },
            new Post
            {
                UserId = users[0].UserId,
                Content = "Debugging my sleep schedule (spoiler: it is still 404). Send caffeine or existential epiphanies. #SwampLife",
                PostType = "image",
                MediaType = "image",
                MediaUrl = "/images/4x3_Photo_of_a_messy_desk_with_table.png",
                AltText = "Photo of a messy desk with tablets, coffee, and a sticky note",
                CreatedAt = now.AddHours(-8)
            },
            new Post
            {
                UserId = users[3].UserId,
                Content = "What is hopping friends? #SwampLife",
                PostType = "text",
                CreatedAt = now.AddHours(-11)
            },
            new Post
            {
                UserId = users[1].UserId,
                Content = "Campus Bonfire Night this Friday at 8PM.",
                PostType = "text",
                CreatedAt = now.AddHours(-3)
            },
            new Post
            {
                UserId = users[1].UserId,
                Content = "Swamp Chess Tournament - March 20th, 4PM at Commons Game Room.",
                PostType = "text",
                CreatedAt = now.AddHours(-6)
            }
        };

        await context.Posts.AddRangeAsync(posts);
        await context.SaveChangesAsync();

        var tagMap = new Dictionary<int, string[]>
        {
            [posts[0].PostId] = ["StudySwamp"],
            [posts[1].PostId] = ["SwampLostAndFound"],
            [posts[2].PostId] = ["TechMagic", "PixelFrog"],
            [posts[3].PostId] = ["SwampMysteries"],
            [posts[4].PostId] = ["ShroomBuddies"],
            [posts[5].PostId] = ["SwampLife"],
            [posts[6].PostId] = ["SwampLife"],
            [posts[7].PostId] = ["CampusLife"],
            [posts[8].PostId] = ["CampusLife"]
        };

        var postTags = new List<PostTag>();
        foreach (var pair in tagMap)
        {
            foreach (var tagName in pair.Value)
            {
                var tag = tags.First(x => x.TagName == tagName);
                postTags.Add(new PostTag { PostId = pair.Key, TagId = tag.TagId });
                tag.UsageCount += 1;
            }
        }

        await context.PostTags.AddRangeAsync(postTags);

        await context.Events.AddAsync(new Event
        {
            PostId = posts[7].PostId,
            EventTime = now.AddDays(2).Date.AddHours(20),
            Location = "South Quad",
            HostOrg = "Student Activities Board",
            RsvpCount = 63,
            MaxCapacity = 120
        });

        await context.Events.AddAsync(new Event
        {
            PostId = posts[8].PostId,
            EventTime = now.AddDays(7).Date.AddHours(16),
            Location = "Commons Game Room",
            HostOrg = "Strategy Club",
            RsvpCount = 42,
            MaxCapacity = 60
        });

        await context.Interactions.AddRangeAsync(
        [
            new Interaction { UserId = users[0].UserId, PostId = posts[0].PostId, InteractionType = "comment", Content = "I am in!", CreatedAt = now.AddMinutes(-22) },
            new Interaction { UserId = users[2].UserId, PostId = posts[0].PostId, InteractionType = "reribb", CreatedAt = now.AddMinutes(-19) },
            new Interaction { UserId = users[0].UserId, PostId = posts[2].PostId, InteractionType = "like", CreatedAt = now.AddMinutes(-90) },
            new Interaction { UserId = users[1].UserId, PostId = posts[2].PostId, InteractionType = "comment", Content = "All hail the #PixelFrog !", CreatedAt = now.AddMinutes(-80) },
            new Interaction { UserId = users[8].UserId, PostId = posts[2].PostId, InteractionType = "comment", Content = "All hail the #PixelFrog !!!", CreatedAt = now.AddMinutes(-70) },
            new Interaction { UserId = users[6].UserId, PostId = posts[2].PostId, InteractionType = "comment", Content = "Legendary.", CreatedAt = now.AddMinutes(-60) },
            new Interaction { UserId = users[7].UserId, PostId = posts[2].PostId, InteractionType = "comment", Content = "Mind if I call? I need help with the biochemistry section!", CreatedAt = now.AddMinutes(-50) },
            new Interaction { UserId = users[2].UserId, PostId = posts[7].PostId, InteractionType = "rsvp", CreatedAt = now.AddMinutes(-40) },
            new Interaction { UserId = users[3].UserId, PostId = posts[8].PostId, InteractionType = "rsvp", CreatedAt = now.AddMinutes(-30) }
        ]);

        await context.SaveChangesAsync();
    }
}
