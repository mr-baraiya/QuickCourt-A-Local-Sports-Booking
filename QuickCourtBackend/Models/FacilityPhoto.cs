using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class FacilityPhoto
{
    public int PhotoId { get; set; }

    public int FacilityId { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public string? Caption { get; set; }

    [JsonIgnore]
    public virtual Facility? Facility { get; set; }
    
}
