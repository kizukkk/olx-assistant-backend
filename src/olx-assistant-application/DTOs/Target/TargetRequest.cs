using olx_assistant_domain.Entities.Common;
using System.Text.Json.Serialization;

namespace olx_assistant_application.DTOs.Target;
public class TargetRequest
{
    [JsonPropertyName("target_uri")]
    public required string TargetUri { get; set; }
    public List<Keyword>? Keywords { get; set; }
}
