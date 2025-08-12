using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public string? AvatarUrl { get; set; }

    public string Role { get; set; } = null!;

    public bool? IsVerified { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore]
    public virtual ICollection<Facility> FacilityApprovedByNavigations { get; set; } = new List<Facility>();

    [JsonIgnore]
    public virtual ICollection<Facility> FacilityOwners { get; set; } = new List<Facility>();

    [JsonIgnore]
    public virtual ICollection<OtpVerification> OtpVerifications { get; set; } = new List<OtpVerification>();

    [JsonIgnore]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
