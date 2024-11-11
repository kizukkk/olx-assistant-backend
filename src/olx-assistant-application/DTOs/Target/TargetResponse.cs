using olx_assistant_domain.Entities.Common;
using olx_assistant_domain.Entities;

namespace olx_assistant_application.DTOs.Target;
public class TargetResponse
{
    public required Uri TargetUri { get; set; }
    public List<Keyword>? Keywords { get; set; }
    public List<Product>? Products { get; set; }
}

