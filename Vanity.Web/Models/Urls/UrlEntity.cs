namespace Vanity.Web.Models.Urls;

/// <summary>
/// Represents a URL entity in the system.
/// </summary>
public class UrlEntity : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LongUrl { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
}
