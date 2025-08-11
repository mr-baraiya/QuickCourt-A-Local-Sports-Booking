using System.Text.Json.Serialization;

namespace QuickCourtBackend.Models;

public partial class OtpVerification
{
    public int OtpId { get; set; }

    public int? UserId { get; set; }
    public string? Email { get; set; }
    public string OtpCode { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool? IsUsed { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
public class VerifyOtpRequest
{
    public int? UserId { get; set; }    // nullable
    public string? Email { get; set; }  // nullable
    public string Purpose { get; set; } = null!;
    public string OtpCode { get; set; } = null!;
}
public class OtpCreationRequest
{
    public int? UserId { get; set; }    // nullable
    public string? Email { get; set; }  // nullable
    public string Purpose { get; set; } = null!;
}