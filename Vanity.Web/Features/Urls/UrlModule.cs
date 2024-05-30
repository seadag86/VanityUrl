using Carter;
using MediatR;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls;

internal class UrlModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            List<UrlResponse> result = await sender.Send(new ListUrlsQuery());

            return Results.Ok();
        });

        app.MapPost("/", (UrlRequest request) =>
        {

            if (Uri.TryCreate(request.Url, UriKind.Absolute, out Uri? _))
            {
                return Results.BadRequest("Invalid URL");
            }

            return Results.Ok();
        });
    }
}
