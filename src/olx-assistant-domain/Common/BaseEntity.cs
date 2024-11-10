namespace olx_assistant_domain.Common;
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset UpdateDate { get; set; } = DateTimeOffset.Now;

}
