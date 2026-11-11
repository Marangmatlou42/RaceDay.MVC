namespace RaceDay.MVC.Models;

public class EnrolmentViewModel
{
    public Guid RegistrationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public EventInfo Event { get; set; } = new();
    public List<EntryInfo> Entries { get; set; } = new();
}

public class EventInfo
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class EntryInfo
{
    public Guid Event_entryId { get; set; }
    public string EntryStatus { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? FinishTime { get; set; }
    public int? FinishingPosition { get; set; }
}