namespace Vanity.Web.Models;

public abstract class Auditable
{
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime ModifiedOn { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
