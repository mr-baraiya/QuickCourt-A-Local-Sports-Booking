using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class Court
{
    public int CourtId { get; set; }

    public int FacilityId { get; set; }

    public int SportId { get; set; }

    public string Name { get; set; } = null!;

    public decimal PricePerHour { get; set; }

    public int? Capacity { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore]
    public virtual Facility? Facility { get; set; }

    [JsonIgnore]
    public virtual Sport? Sport { get; set; }

    [JsonIgnore]
    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}
