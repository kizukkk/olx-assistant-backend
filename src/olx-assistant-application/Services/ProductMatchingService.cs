using olx_assistant_application.DTOs.Shared;
using olx_assistant_domain.Entities;
using olx_assistant_scraping;

namespace olx_assistant_application.Services;
public static class ProductMatchingService
{

    public static async Task<List<ProductResponse>> StartMatchingByTarget(Target target)
    {
        Uri paginatedUrl = new Uri($"{target.TargetUri}/?page={1}");

        //TODO: save product list and map to ProductResponse using Mapper
        await ProductScraping.GetProductListAsync(paginatedUrl);

        return new List<ProductResponse>();
    }
}
