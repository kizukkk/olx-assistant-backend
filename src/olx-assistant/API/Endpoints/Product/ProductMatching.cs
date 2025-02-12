using olx_assistant_application.Interfaces.IServices;
using olx_assistant_application.DTOs.Shared;
using olx_assistant_domain.Entities.Common;
using FastEndpoints;

namespace olx_assistant_api.Endpoints.Product;

public class ProductMatching : EndpointWithoutRequest
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
            Keywords = new List<Keyword> { new("Трактор", 1f), new("Тракторець", .9f) }
        };


        _matchingService.StartFastMatchingByTarget(target);

       await SendOkAsync();
    }

}
