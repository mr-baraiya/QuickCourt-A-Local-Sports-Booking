using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class TimeSlot
{
    public int TimeSlotId { get; set; }

    public int CourtId { get; set; }

    public DateOnly SlotDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Status { get; set; }

    [JsonIgnore]
    public virtual Court? Court { get; set; }
}
