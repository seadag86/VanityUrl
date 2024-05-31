using Carter;
using MediatR;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls;

public class UrlModule : CarterModule
{
    private readonly ISender _sender;

    public UrlModule(ISender sender)
    {
        _sender = sender;
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// Handles the GET request at the root ("/") endpoint.
        /// </summary>
        /// <returns>A list of UrlResponse objects wrapped in an Ok result.</returns>
        app.MapGet("/", async () =>
        {
            List<UrlResponse> result = await _sender.Send(new ListUrlsQuery());

            return Results.Ok();
        })
            .WithName("GetUrls")
            .WithTags("Urls")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all shortened or beautified URLs",
                Description = "Get a list of all shortened or beautified URLs and their corresponding 'original' URLs"
            });

        /// <summary>
        /// Handles the POST request at the root ("/") endpoint.
        /// </summary>
        /// <param name="request">The request containing the URL to be processed.</param>
        /// <returns>An Ok result if the URL is valid, otherwise a BadRequest result with an error message.</returns>
        app.MapPost("/", (UrlRequest request) =>
        {

            if (Uri.TryCreate(request.Url, UriKind.Absolute, out Uri? _))
            {
                return Results.BadRequest("Invalid URL");
            }

            return Results.Ok();
        })
            .WithName("PostUrl")
            .WithTags("Urls")
            .WithOpenApi(operation => new(operation)
            {
                // summary and description
                Summary = "Redirect to URL using shortened or beautified URL",
                Description = "Pass in a URL with random code or a vanity URL and be redirected to corresponding 'orgininal' URL"
            });
    }
}
