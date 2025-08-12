using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class Facility
{
    public int FacilityId { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Address { get; set; } = null!;

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Status { get; set; }

    public string? RejectionReason { get; set; }

    public TimeOnly? OperatingHoursStart { get; set; }

    public TimeOnly? OperatingHoursEnd { get; set; }

    public int? ApprovedBy { get; set; }

    public string? ApprovalComments { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual User? ApprovedByNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();

    [JsonIgnore]
    public virtual ICollection<FacilityPhoto> FacilityPhotos { get; set; } = new List<FacilityPhoto>();

    [JsonIgnore]
    public virtual User? Owner { get; set; }

    [JsonIgnore]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [JsonIgnore]
    public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}
