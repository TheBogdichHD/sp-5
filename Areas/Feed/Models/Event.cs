namespace Lab5.Areas.Feed.Models;

public sealed class Event
{
    public int EventId { get; set; }
    public int PostId { get; set; }
    public DateTime EventTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? HostOrg { get; set; }
    public int RsvpCount { get; set; }
    public int? MaxCapacity { get; set; }

    public Post Post { get; set; } = null!;
}
