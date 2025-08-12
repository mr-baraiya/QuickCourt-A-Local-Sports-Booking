using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class Sport
{
    public int SportId { get; set; }

    public string Name { get; set; } = null!;

    public string? IconUrl { get; set; }

    [JsonIgnore]
    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
}
