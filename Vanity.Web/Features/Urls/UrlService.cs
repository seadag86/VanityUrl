using MediatR;
using System.Text.RegularExpressions;
using Vanity.Web.Data;
using Vanity.Web.Features.Urls.Queries;

namespace Vanity.Web.Features.Urls;

public class UrlService : IUrlService
{
    public const int CodeLength = 7;
    private const string CodeAlphabet = "abcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new Random();
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public UrlService(ApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<string> GenerateRandomUrlCode()
    {
        var codeCharactrs = new char[CodeLength];

        while(true)
        {
            for (int i = 0; i < CodeLength; i++)
            {
                var randomIndex = _random.Next(CodeAlphabet.Length - 1);
                codeCharactrs[i] = CodeAlphabet[randomIndex];
            }

            var code = new string(codeCharactrs);
            var existingUrl = await _mediator.Send(new GetUrlByAliasQuery(code));
            
            if (existingUrl == null) {
                return code;
            }
        }
    }

    public string StringToAlias(string input)
    {
        // Remove all non-alphanumeric characters and replace all spaces with hyphens
        var alias = Regex.Replace(input.ToLower(), @"[^a-zA-Z0-9\s]", "").Replace(" ", "-");

        return alias;
    }
}
