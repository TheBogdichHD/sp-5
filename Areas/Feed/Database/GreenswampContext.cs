using Lab5.Areas.Feed.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Areas.Feed.Database;

public class GreenswampContext(DbContextOptions<GreenswampContext> options) : DbContext(options)
{
    public DbSet<Auth> Auths => Set<Auth>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Interaction> Interactions => Set<Interaction>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostTag> PostTags => Set<PostTag>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auth>(entity =>
        {
            entity.ToTable("auth");
            entity.HasKey(x => x.UserId);
            entity.Property(x => x.UserId).HasColumnName("user_id").ValueGeneratedNever();
            entity.Property(x => x.PasswordHash).HasColumnName("password_hash");
            entity.Property(x => x.LastLogin).HasColumnType("DATETIME").HasColumnName("last_login");
            entity.Property(x => x.ResetToken).HasColumnName("reset_token");
            entity.Property(x => x.TokenExpiry).HasColumnType("DATETIME").HasColumnName("token_expiry");

            entity.HasOne(x => x.User)
                .WithOne(x => x.Auth)
                .HasForeignKey<Auth>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.UserId);
            entity.Property(x => x.UserId).HasColumnName("user_id");
            entity.Property(x => x.Username).HasColumnName("username");
            entity.Property(x => x.DisplayName).HasColumnName("display_name");
            entity.Property(x => x.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(x => x.Bio).HasColumnName("bio");
            entity.Property(x => x.CreatedAt).HasColumnType("DATETIME").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(x => x.IsActive).HasColumnType("BOOLEAN").HasColumnName("is_active").HasDefaultValue(true);
            entity.HasIndex(x => x.Username).IsUnique();
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
            entity.Property(x => x.CreatedAt).HasColumnType("DATETIME").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(x => x.ParentPostId).HasColumnName("parent_post_id");

            entity.HasIndex(x => x.CreatedAt).HasDatabaseName("idx_posts_created");
            entity.HasIndex(x => x.PostType).HasDatabaseName("idx_posts_type");

            entity.HasOne(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.ParentPost)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentPostId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(x => x.EventId);
            entity.Property(x => x.EventId).HasColumnName("event_id");
            entity.Property(x => x.PostId).HasColumnName("post_id");
            entity.Property(x => x.EventTime).HasColumnType("DATETIME").HasColumnName("event_time");
            entity.Property(x => x.Location).HasColumnName("location");
            entity.Property(x => x.HostOrg).HasColumnName("host_org");
            entity.Property(x => x.RsvpCount).HasColumnName("rsvp_count").HasDefaultValue(0L);
            entity.Property(x => x.MaxCapacity).HasColumnName("max_capacity");

            entity.HasIndex(x => x.EventTime).HasDatabaseName("idx_events_time");
            entity.HasIndex(x => x.PostId).IsUnique();

            entity.HasOne(x => x.Post)
                .WithOne(x => x.Event)
                .HasForeignKey<Event>(x => x.PostId)
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
            entity.Property(x => x.CreatedAt).HasColumnType("DATETIME").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(x => x.Content).HasColumnName("content");

            entity.HasIndex(x => x.InteractionType).HasDatabaseName("idx_interactions_type");
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

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("tags");
            entity.HasKey(x => x.TagId);
            entity.Property(x => x.TagId).HasColumnName("tag_id");
            entity.Property(x => x.TagName).HasColumnName("tag_name");
            entity.Property(x => x.CreatedAt).HasColumnType("DATETIME").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(x => x.UsageCount).HasColumnName("usage_count").HasDefaultValue(0L);

            entity.HasIndex(x => x.TagName).HasDatabaseName("idx_tags_name").IsUnique();
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

    }
}
