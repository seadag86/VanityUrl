using MediatR;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls;

public sealed record ListUrlsQuery : IRequest<List<UrlResponse>>
{
    public string Url { get; init; } = string.Empty;
}

public sealed class ListUrlsQueryHandler : IRequestHandler<ListUrlsQuery, List<UrlResponse>>
{
    public async Task<List<UrlResponse>> Handle(ListUrlsQuery request, CancellationToken cancellationToken)
    {
        List<UrlResponse> foo = new();
        return await Task.FromResult(foo);
    }
}

