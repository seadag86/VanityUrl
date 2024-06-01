using MediatR;
using Microsoft.EntityFrameworkCore;
using Vanity.Web.Data;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls.Queries;

public sealed record GetUrlByAliasQuery(string alias) : IRequest<UrlEntity>;

public class GetUrlByAliasHandler : IRequestHandler<GetUrlByAliasQuery, UrlEntity?>
{
    private readonly ApplicationDbContext _context;

    public GetUrlByAliasHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UrlEntity?> Handle(GetUrlByAliasQuery request, CancellationToken cancellationToken)
    {
        var urlEntity = await _context.Urls.AsNoTracking().FirstOrDefaultAsync(u => u.Alias == request.alias, cancellationToken);

        return urlEntity;
    }
}
