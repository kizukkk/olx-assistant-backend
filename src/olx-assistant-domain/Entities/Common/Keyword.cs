namespace olx_assistant_domain.Entities.Common;
public class Keyword : BaseEntity
{
    public required string Word { get; set; }
    public int Value { get; set; }

}
