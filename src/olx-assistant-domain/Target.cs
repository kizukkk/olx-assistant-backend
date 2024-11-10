using olx_assistant_domain.Common;

namespace olx_assistant_domain;
public class Target : BaseEntity
{
    public required Uri TargetUri { get; set; }
    public List<Keyword>? Keywords { get; set; }

}
