namespace olx_assistant_domain.Entities.Common;
public class TargetTask
{
    public int TaskId { get; set; }
    public int TargetId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

}
