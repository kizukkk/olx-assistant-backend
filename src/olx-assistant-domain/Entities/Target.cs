using olx_assistant_domain.Entities.Common;

namespace olx_assistant_domain.Entities;
public class Target : BaseEntity
{
    public required Uri TargetUri { get; set; }
    public List<Keyword>? Keywords { get; set; }

}
