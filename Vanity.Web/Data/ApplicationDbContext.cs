using Microsoft.EntityFrameworkCore;
using Vanity.Web.Features.Urls;
using Vanity.Web.Models.Urls;

namespace Vanity.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UrlEntity> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Url).IsRequired();
            entity.Property(e => e.Alias).IsRequired();
            entity.Property(e => e.Alias).HasMaxLength(UrlService.CodeLength);
            entity.HasIndex(e => e.Alias).IsUnique();
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();
        });
    }
}
