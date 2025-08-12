using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int CourtId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Status { get; set; }

    public string? PaymentStatus { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Court? Court { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
