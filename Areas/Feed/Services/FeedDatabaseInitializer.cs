using Lab5.Areas.Feed.Database;
using Lab5.Areas.Feed.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Areas.Feed.Services;

public class FeedDatabaseInitializer(GreenswampContext db)
{
    public async Task InitializeAsync()
    {
        await db.Database.EnsureCreatedAsync();

        if (await db.Users.AnyAsync())
        {
            return;
        }

        await SeedDemoDataAsync();
    }

    private async Task SeedDemoDataAsync()
    {
        var users = new List<User>
        {
            new User
            {
                Username = "frogger_vig",
                DisplayName = "Viggo The Frogger",
                AvatarUrl = "https://i.pravatar.cc/100?u=frogger_vig@pravatar.com",
                Bio = "Digital artist exploring the cyber frontier •\nCreator of neural interfaces •\nPosts about tech, art, and the future",
                IsActive = true
            },
            new User
            {
                Username = "biology_buff",
                DisplayName = "BioBuddy",
                AvatarUrl = "https://i.pravatar.cc/100?u=biology_buff@pravatar.com",
                Bio = "Biology enjoyer.",
                IsActive = true
            },
            new User
            {
                Username = "chem_frog",
                DisplayName = "ChemFrog",
                AvatarUrl = "https://i.pravatar.cc/100?u=chem_frog@pravatar.com",
                Bio = "Chemistry and frogs.",
                IsActive = true
            },
            new User
            {
                Username = "prcs_toad",
                DisplayName = "ToadPrincess",
                AvatarUrl = "https://i.pravatar.cc/100?u=prcs_toad@pravatar.com",
                Bio = "Princess of the pond.",
                IsActive = true
            },
            new User
            {
                Username = "dorm23_frogs",
                DisplayName = "FroggyFriends",
                AvatarUrl = "https://i.pravatar.cc/100?u=dorm23_frogs@pravatar.com",
                Bio = "Campus swamp chatter and study sessions.",
                IsActive = true
            },
            new User
            {
                Username = "ptst_toad",
                DisplayName = "Princess Toadstool",
                AvatarUrl = "https://i.pravatar.cc/100?u=ptst_toad@pravatar.com",
                Bio = "What's hopping friends?",
                IsActive = true
            },
            new User
            {
                Username = "lostnfound",
                DisplayName = "SwampFinder",
                AvatarUrl = "https://i.pravatar.cc/100?u=swamp_finder@pravatar.com",
                Bio = "Lost and found around the bog.",
                IsActive = true
            },
            new User
            {
                Username = "mycelium_maddy",
                DisplayName = "FungiFanatic",
                AvatarUrl = "https://i.pravatar.cc/100?u=shroom_lady@pravatar.com",
                Bio = "Collector of suspicious mushrooms.",
                IsActive = true
            },
            new User
            {
                Username = "actual_swamp_monster",
                DisplayName = "MossBeard99",
                AvatarUrl = "https://i.pravatar.cc/100?u=swamp_cryptid@pravatar.com",
                Bio = "Cryptid watcher.",
                IsActive = true
            }
        };

        for (var i = 1; i <= 400; i++)
        {
            users.Add(new User
            {
                Username = $"comment_frog_{i}",
                DisplayName = $"Comment Frog {i}",
                AvatarUrl = $"https://i.pravatar.cc/100?u=comment_frog_{i}@pravatar.com",
                Bio = "Ribbiting in comments.",
                IsActive = true
            });
        }

        db.Users.AddRange(users);
        await db.SaveChangesAsync();

        var frogger = users.First(x => x.Username == "frogger_vig");
        var bioBuddy = users.First(x => x.Username == "biology_buff");
        var chemFrog = users.First(x => x.Username == "chem_frog");
        var toadPrincess = users.First(x => x.Username == "prcs_toad");
        var froggyFriends = users.First(x => x.Username == "dorm23_frogs");
        var princessToadstool = users.First(x => x.Username == "ptst_toad");
        var swampFinder = users.First(x => x.Username == "lostnfound");
        var fungiFanatic = users.First(x => x.Username == "mycelium_maddy");
        var mossBeard = users.First(x => x.Username == "actual_swamp_monster");

        var posts = new List<Post>
        {
            new Post
            {
                UserId = frogger.UserId,
                Content = "When the code compiles on the first try 🐸✨ #TechMagic #PixelFrog",
                PostType = "video",
                MediaUrl = "/media/Standard_Mode_a_looed_gif_of_a_pixel_frog_flyi.mp4",
                MediaType = "video",
                ThumbnailUrl = "/images/green-toad-logo.svg",
                AltText = "Digital painting of a neon frog piloting a cyberpunk airship",
                CreatedAt = DateTime.UtcNow.AddHours(-2)
            },
            new Post
            {
                UserId = bioBuddy.UserId,
                Content = "I can't believe it's working! #TechMagic",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddHours(-6)
            },
            new Post
            {
                UserId = frogger.UserId,
                Content = "Debugging my sleep schedule (spoiler: it's still 404). Send caffeine or existential epiphanies.",
                PostType = "image",
                MediaUrl = "/images/4x3_Photo_of_a_messy_desk_with_table.png",
                MediaType = "image",
                AltText = "Photo of a messy desk with tablets, coffee",
                CreatedAt = DateTime.UtcNow.AddHours(-8)
            },
            new Post
            {
                UserId = bioBuddy.UserId,
                Content = "All hail the #PixelFrog !",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddMinutes(-22)
            },
            new Post
            {
                UserId = toadPrincess.UserId,
                Content = "All hail the #PixelFrog !!!",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddMinutes(-20)
            },
            new Post
            {
                UserId = chemFrog.UserId,
                Content = "Mind if I call? I need help with the biochemistry section!",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddMinutes(-15)
            },
            new Post
            {
                UserId = froggyFriends.UserId,
                Content = "Anyone want to form a study group for the biology midterm? 🐸 Meeting at the lilypad cafe tomorrow 3PM! #StudySwamp",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddMinutes(-29)
            },
            new Post
            {
                UserId = froggyFriends.UserId,
                Content = "Campus Bonfire Night",
                PostType = "event",
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            },
            new Post
            {
                UserId = princessToadstool.UserId,
                Content = "What's hopping friends? ☂️ #SwampLife",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddHours(-11)
            },
            new Post
            {
                UserId = swampFinder.UserId,
                Content = "🚨 LOST: Silver water bottle with algae sticker near Turtle Pond. Reward in bug cookies! 🍪 #SwampLostAndFound",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddMinutes(-45)
            },
            new Post
            {
                UserId = mossBeard.UserId,
                Content = "Swamp Chess Tournament",
                PostType = "event",
                CreatedAt = DateTime.UtcNow.AddHours(-6)
            },
            new Post
            {
                UserId = fungiFanatic.UserId,
                Content = "Just found a glowing mushroom in the North Bog! 🍄✨ PSA: Do NOT lick it. We tried. Dean's office was... unamused. #ShroomBuddies",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddMinutes(-17)
            },
            new Post
            {
                UserId = mossBeard.UserId,
                Content = "👣 Found weird footprints by the old dock! Either: 1) New cryptid friend 🦎 Or 2) Dave forgot his waders again #SwampMysteries",
                PostType = "text",
                CreatedAt = DateTime.UtcNow.AddHours(-1)
            }
        };

        db.Posts.AddRange(posts);
        await db.SaveChangesAsync();

        var mainPost = posts[0];
        var secondPost = posts[1];
        var debugPost = posts[2];
        var replyOne = posts[3];
        var replyTwo = posts[4];
        var nestedReply = posts[5];
        var studyGroupPost = posts[6];
        var bonfirePost = posts[7];
        var toadstoolPost = posts[8];
        var lostFoundPost = posts[9];
        var chessPost = posts[10];
        var mushroomPost = posts[11];
        var cryptidPost = posts[12];

        replyOne.ParentPostId = mainPost.PostId;
        replyTwo.ParentPostId = mainPost.PostId;
        nestedReply.ParentPostId = replyOne.PostId;
        db.Posts.UpdateRange(replyOne, replyTwo, nestedReply);
        await db.SaveChangesAsync();

        db.Events.AddRange(
            new Event
            {
                PostId = bonfirePost.PostId,
                EventTime = DateTime.UtcNow.AddDays(5).Date.AddHours(20),
                Location = "South Quad",
                HostOrg = "Student Activities Board",
                RsvpCount = 63,
                MaxCapacity = 120
            },
            new Event
            {
                PostId = chessPost.PostId,
                EventTime = DateTime.UtcNow.AddDays(7).Date.AddHours(16),
                Location = "Commons Game Room",
                HostOrg = "Strategy Club",
                RsvpCount = 14,
                MaxCapacity = 32
            });

        var tags = new[]
        {
            new Tag { TagName = "TechMagic", UsageCount = 2 },
            new Tag { TagName = "PixelFrog", UsageCount = 1 },
            new Tag { TagName = "StudySwamp", UsageCount = 1 },
            new Tag { TagName = "SwampLife", UsageCount = 1 },
            new Tag { TagName = "SwampLostAndFound", UsageCount = 1 },
            new Tag { TagName = "ShroomBuddies", UsageCount = 1 },
            new Tag { TagName = "SwampMysteries", UsageCount = 1 }
        };

        db.Tags.AddRange(tags);
        await db.SaveChangesAsync();

        db.PostTags.AddRange(
            new PostTag { PostId = mainPost.PostId, TagId = tags[0].TagId },
            new PostTag { PostId = mainPost.PostId, TagId = tags[1].TagId },
            new PostTag { PostId = secondPost.PostId, TagId = tags[0].TagId },
            new PostTag { PostId = replyOne.PostId, TagId = tags[1].TagId },
            new PostTag { PostId = replyTwo.PostId, TagId = tags[1].TagId },
            new PostTag { PostId = studyGroupPost.PostId, TagId = tags[2].TagId },
            new PostTag { PostId = toadstoolPost.PostId, TagId = tags[3].TagId },
            new PostTag { PostId = lostFoundPost.PostId, TagId = tags[4].TagId },
            new PostTag { PostId = mushroomPost.PostId, TagId = tags[5].TagId },
            new PostTag { PostId = cryptidPost.PostId, TagId = tags[6].TagId });

        var audienceUsers = users.Where(x => x.Username.StartsWith("comment_frog_")).ToList();
        var audienceIndex = 0;

        var interactions = new List<Interaction>();

        void AddPostInteractions(Post post, int answersCount, int reribbsCount)
        {
            for (var i = 0; i < answersCount; i++)
            {
                var audience = audienceUsers[audienceIndex++ % audienceUsers.Count];
                interactions.Add(new Interaction
                {
                    UserId = audience.UserId,
                    PostId = post.PostId,
                    InteractionType = "comment",
                    Content = "Ribbit!"
                });
            }

            for (var i = 0; i < reribbsCount; i++)
            {
                var audience = audienceUsers[audienceIndex++ % audienceUsers.Count];
                interactions.Add(new Interaction
                {
                    UserId = audience.UserId,
                    PostId = post.PostId,
                    InteractionType = "reribb"
                });
            }
        }

        AddPostInteractions(mainPost, 2, 1);
        AddPostInteractions(secondPost, 10, 2);
        AddPostInteractions(debugPost, 10, 2);
        AddPostInteractions(replyOne, 1, 0);
        AddPostInteractions(studyGroupPost, 12, 6);
        AddPostInteractions(toadstoolPost, 2, 0);
        AddPostInteractions(lostFoundPost, 5, 0);
        AddPostInteractions(mushroomPost, 42, 29);
        AddPostInteractions(cryptidPost, 87, 203);

        db.Interactions.AddRange(interactions);

        await db.SaveChangesAsync();
    }
}
