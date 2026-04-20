namespace Lab5.Areas.Feed.Models;

public class Auth
{
    public long UserId { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime? LastLogin { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? TokenExpiry { get; set; }

    public User User { get; set; } = null!;
}
