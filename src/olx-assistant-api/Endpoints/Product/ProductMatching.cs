using olx_assistant_application.DTOs.Shared;
using olx_assistant_application.Interfaces;
using FastEndpoints;

namespace olx_assistant_api.Endpoints.Product;

public class ProductMatching : EndpointWithoutRequest<List<ProductResponse>>
{

    private readonly IProductMatchingService _matchingService;

    public ProductMatching(IProductMatchingService matchingService) => 
        _matchingService = matchingService;

    public override void Configure()
    {
        Get("/product/matching");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var target = new olx_assistant_domain.Entities.Target()
        {
            TargetUri = new Uri("https://www.olx.ua/uk/transport/selhoztehnika"),
        };

        await ProductMatchingService.StartMatchingByTarget(target);

        await SendAsync( new
        {

        });
    }

}
