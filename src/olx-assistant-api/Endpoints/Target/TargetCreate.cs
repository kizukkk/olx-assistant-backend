using olx_assistant_application.DTOs.Target;
using FastEndpoints;

namespace olx_assistant_api.Endpoints.Target;

public class TargetCreate : Endpoint<TargetRequest>
{
    public override void Configure()
    {
        Post("/target/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TargetRequest req, CancellationToken token)
    {
        await SendAsync(new TargetResponse()
        {
            TargetUri = new Uri(req.TargetUri),
        });
    }
}
