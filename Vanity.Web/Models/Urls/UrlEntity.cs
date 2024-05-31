namespace Vanity.Web.Models.Urls;

/// <summary>
/// Represents a URL entity in the system.
/// </summary>
public class UrlEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the URL entity.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the URL of the entity.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alias of the URL.
    /// </summary>
    public string Alias { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time the URL entity was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the URL entity.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}
