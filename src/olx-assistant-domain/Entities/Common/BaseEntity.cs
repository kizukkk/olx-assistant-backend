namespace olx_assistant_domain.Entities.Common;
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset UpdateDate { get; set; } = DateTimeOffset.Now;

}
