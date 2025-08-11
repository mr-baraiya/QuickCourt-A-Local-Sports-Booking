using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public int FacilityId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Facility? Facility { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
