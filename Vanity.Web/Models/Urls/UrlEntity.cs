namespace Vanity.Web.Models.Urls;

public class UrlEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
