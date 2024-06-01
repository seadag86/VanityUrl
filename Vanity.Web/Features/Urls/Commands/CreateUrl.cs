using MediatR;
using Vanity.Web.Data;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Features.Urls.Commands;

public sealed record CreateUrlCommand(
    string longUrl, 
    string shortUrlBase, 
    string alias
) : IRequest<string>;

public class CreateUrlHandler : IRequestHandler<CreateUrlCommand, string>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateUrlHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(CreateUrlCommand request, CancellationToken cancellationToken)
    {
        var newUrlEntity = new UrlEntity
        {
            LongUrl = request.longUrl,
            Alias = request.alias ?? string.Empty
        };

        _dbContext.Urls.Add(newUrlEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return $"{request.shortUrlBase}/{newUrlEntity.Alias}";
    }
}