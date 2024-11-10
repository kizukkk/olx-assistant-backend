using olx_assistant_domain.Common;

namespace olx_assistant_domain;
public class Product : BaseEntity
{
    public int ProductId {  get; set; }
    public string? Title { get; set; }
    public string? Description { get; set;}
    public string? Place { get; set;}
    public decimal Price { get; set; }
    public int Rating { get; set; }
    public required Target SearchTarget { get; set; }
    public List<Tag>? Tags { get; set; }

}
