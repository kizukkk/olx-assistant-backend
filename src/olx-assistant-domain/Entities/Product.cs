using olx_assistant_domain.Entities.Common;

namespace olx_assistant_domain.Entities;
public class Product : BaseEntity
{
    public int ProductId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public int Rating { get; set; } = 0;
    public Target? SearchTarget { get; set; }
    public List<Tag>? Tags { get; set; }

}
