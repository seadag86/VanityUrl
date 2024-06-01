using MediatR;
using Microsoft.EntityFrameworkCore;
using Vanity.Web.Data;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls.Queries;

public sealed record ListUrlsQuery : IRequest<List<UrlEntity>>;

public sealed class ListUrlsHandler : IRequestHandler<ListUrlsQuery, List<UrlEntity>>
{
    private readonly ApplicationDbContext _dbContent;

    public ListUrlsHandler(ApplicationDbContext dbContent)
    {
        _dbContent = dbContent;
    }

    public async Task<List<UrlEntity>> Handle(ListUrlsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContent.Urls.AsNoTracking().ToListAsync(cancellationToken);
    }
}

