namespace Lab5.Areas.Feed.Models;

public class Event
{
    public long EventId { get; set; }
    public long? PostId { get; set; }
    public DateTime EventTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? HostOrg { get; set; }
    public long? RsvpCount { get; set; }
    public long? MaxCapacity { get; set; }

    public Post? Post { get; set; }
}
