using FastEndpoints;
using olx_assistant_application.Services;

namespace olx_assistant_api.Endpoints.Product;

public class ProductMatching : EndpointWithoutRequest
{
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
