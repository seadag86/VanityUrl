namespace Vanity.Web.Features.Urls;

internal interface IUrlService
{
    Task<string> GenerateRandomUrlCode();
    string StringToAlias(string input);
}