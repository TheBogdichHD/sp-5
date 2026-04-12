using Lab5.Areas.Feed.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Areas.Feed.Data;

public sealed class GreenswampContext(DbContextOptions<GreenswampContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<PostTag> PostTags => Set<PostTag>();
    public DbSet<Interaction> Interactions => Set<Interaction>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<TrendingPond> TrendingPonds => Set<TrendingPond>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.UserId);
            entity.Property(x => x.UserId).HasColumnName("user_id");
            entity.Property(x => x.Username).HasColumnName("username");
            entity.Property(x => x.DisplayName).HasColumnName("display_name");
            entity.Property(x => x.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(x => x.Bio).HasColumnName("bio");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.IsActive).HasColumnName("is_active");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("posts");
            entity.HasKey(x => x.PostId);
            entity.Property(x => x.PostId).HasColumnName("post_id");
            entity.Property(x => x.UserId).HasColumnName("user_id");
            entity.Property(x => x.Content).HasColumnName("content");
            entity.Property(x => x.PostType).HasColumnName("post_type");
            entity.Property(x => x.MediaUrl).HasColumnName("media_url");
            entity.Property(x => x.MediaType).HasColumnName("media_type");
            entity.Property(x => x.AltText).HasColumnName("alt_text");
            entity.Property(x => x.ThumbnailUrl).HasColumnName("thumbnail_url");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.ParentPostId).HasColumnName("parent_post_id");

            entity.HasOne(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.ParentPost)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentPostId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("tags");
            entity.HasKey(x => x.TagId);
            entity.Property(x => x.TagId).HasColumnName("tag_id");
            entity.Property(x => x.TagName).HasColumnName("tag_name");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.UsageCount).HasColumnName("usage_count");
        });

        modelBuilder.Entity<PostTag>(entity =>
        {
            entity.ToTable("post_tags");
            entity.HasKey(x => new { x.PostId, x.TagId });
            entity.Property(x => x.PostId).HasColumnName("post_id");
            entity.Property(x => x.TagId).HasColumnName("tag_id");

            entity.HasOne(x => x.Post)
                .WithMany(x => x.PostTags)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Tag)
                .WithMany(x => x.PostTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Interaction>(entity =>
        {
            entity.ToTable("interactions");
            entity.HasKey(x => x.InteractionId);
            entity.Property(x => x.InteractionId).HasColumnName("interaction_id");
            entity.Property(x => x.UserId).HasColumnName("user_id");
            entity.Property(x => x.PostId).HasColumnName("post_id");
            entity.Property(x => x.InteractionType).HasColumnName("interaction_type");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.Content).HasColumnName("content");

            entity.HasIndex(x => new { x.UserId, x.PostId, x.InteractionType }).IsUnique();

            entity.HasOne(x => x.User)
                .WithMany(x => x.Interactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Post)
                .WithMany(x => x.Interactions)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(x => x.EventId);
            entity.Property(x => x.EventId).HasColumnName("event_id");
            entity.Property(x => x.PostId).HasColumnName("post_id");
            entity.Property(x => x.EventTime).HasColumnName("event_time");
            entity.Property(x => x.Location).HasColumnName("location");
            entity.Property(x => x.HostOrg).HasColumnName("host_org");
            entity.Property(x => x.RsvpCount).HasColumnName("rsvp_count");
            entity.Property(x => x.MaxCapacity).HasColumnName("max_capacity");

            entity.HasOne(x => x.Post)
                .WithOne(x => x.Event)
                .HasForeignKey<Event>(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TrendingPond>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("trending_ponds");
            entity.Property(x => x.TagId).HasColumnName("tag_id");
            entity.Property(x => x.TagName).HasColumnName("tag_name");
            entity.Property(x => x.RecentPosts).HasColumnName("recent_posts");
        });
    }
}
