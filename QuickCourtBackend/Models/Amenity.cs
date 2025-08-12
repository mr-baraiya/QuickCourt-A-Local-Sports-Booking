using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class Amenity
{
    public int AmenityId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();
}
