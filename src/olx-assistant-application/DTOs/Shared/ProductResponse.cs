using olx_assistant_domain.Entities.Common;

namespace olx_assistant_application.DTOs.Shared;
public class ProductResponse
{
    public int ProductId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Place { get; set; }
    public decimal Price { get; set; } = 0;
    public int Rating { get; set; } = 0;
    public List<Tag>? Tags { get; set; }
}
