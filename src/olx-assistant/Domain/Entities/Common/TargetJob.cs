namespace olx_assistant_domain.Entities.Common;
public class TargetJob
{
    public string? jobId { get; set; }
    public int TargetId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

}
