using Carter;
using MediatR;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls;

internal class UrlModule : CarterModule
{
    public UrlModule()
        : base("/")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            List<UrlResponse> result = await sender.Send(new ListUrlsQuery());

            return Results.Ok();
        });

        app.MapPost("/", (string url, ISender sender) =>
        {

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? _))
            {
                return Results.BadRequest("Invalid URL");
            }

            return Results.Ok();
        });
    }
}
