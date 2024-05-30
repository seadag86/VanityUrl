using System.Text.RegularExpressions;
using Vanity.Web.Data;

namespace Vanity.Web.Features.Urls;

internal class UrlService
{
    public const int CodeLength = 7;
    private const string CodeAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    private readonly Random _random = new Random();
    private readonly ApplicationDbContext _dbContext;

    public UrlService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string GenerateRandomUrlCode()
    {
        var codeCharactrs = new char[CodeLength];

        for (int i = 0; i < CodeLength; i++)
        {
            var randomIndex = _random.Next(CodeAlphabet.Length - 1);
            codeCharactrs[i] = CodeAlphabet[randomIndex];
        }

        var code = new string(codeCharactrs);
        return code;
    }

    public string StringToAlias(string input)
    {
        // Remove all non-alphanumeric characters and replace all spaces with hyphens
        var alias = Regex.Replace(input, @"[^a-zA-Z0-9\s]", "").Replace(" ", "-");
        
        return alias;
    }
}
