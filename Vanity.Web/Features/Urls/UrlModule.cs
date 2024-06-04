using Carter;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Vanity.Web.Features.Urls.Commands;
using Vanity.Web.Features.Urls.Queries;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls;

public class UrlModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// Gets a list of urls in the database.
        /// </summary>
        /// <returns>A list of UrlResponse objects wrapped in an Ok result.</returns>
        app.MapGet("/api/urls", async (IMediator _mediator) =>
        {
            List<UrlEntity> result = await _mediator.Send(new ListUrlsQuery());

            return Results.Ok(result);
        })
            .WithName("GetUrls")
            .WithTags("Urls")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all shortened or beautified URLs",
                Description = "List all shortened or beautified URLs and their corresponding 'original' URLs"
            })
            .CacheOutput(x => x.Tag("urls"));

        /// <summary>
        /// Gets the shortened URL by the alias and redirects the request.
        /// </summary>
        app.MapGet("/{alias}", async (string alias, IMediator mediator) =>
        {
            UrlEntity urlENtity = await mediator.Send(new GetUrlByAliasQuery(alias));

            if (urlENtity == null)
            {
                return Results.NotFound();
            }

            // redirect to shortened url
            return Results.Redirect(urlENtity.LongUrl);
        })
            .WithName("GetUrlByAlias")
            .WithTags("Urls")
            .WithOpenApi(operation => new(operation)
            {
                // summary and description
                Summary = "Get a shortened URL by the alias",
                Description = "Pass in a URL with optional alias to be redirected to shortened URL"
            })
            .CacheOutput(x => x.Tag("urls"));

        /// <summary>
        /// Create a shortened URL with a randome code or specified alias.
        /// </summary>
        /// <param name="request">The request containing the URL to be processed.</param>
        /// <returns>An Ok result if the URL is valid, otherwise a BadRequest result with an error message.</returns>
        app.MapPost("/api/urls", async (
            UrlRequest request, 
            IMediator mediator, 
            UrlService urlService, 
            IOutputCacheStore cacheStore,
            HttpContext httpContext,
            CancellationToken cancellationToken
        ) =>
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out Uri? _))
            {
                return Results.BadRequest("Invalid URL");
            }

            string urlCode = string.IsNullOrEmpty(request.Alias) 
                ? await urlService.GenerateRandomUrlCode()
                : urlService.StringToAlias(request.Alias);
            string shortUrlBase = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            var shortenedUrl = await mediator.Send(new CreateUrlCommand(request.Url, shortUrlBase, urlCode));

            await cacheStore.EvictByTagAsync("urls", cancellationToken);

            return Results.Ok(shortenedUrl);
        })
            .WithName("CreateUrlWithCode")
            .WithTags("Urls")
            .WithOpenApi(operation => new(operation)
            {
                // summary and description
                Summary = "Create a shortened URL with a random code",
                Description = "Pass in a URL with optional alias to create a new shortened URL"
            });
    }
}
