using Microsoft.EntityFrameworkCore;
using Vanity.Web.Features.Urls;
using Vanity.Web.Models;
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
            entity.Property(e => e.LongUrl).IsRequired();
            entity.Property(e => e.Alias).IsRequired();
            entity.HasIndex(e => e.Alias).IsUnique();
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();
        });
    }

    // When saving URLEntity, set the CreatedOn and UpdatedOn properties
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Auditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "foo";
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = "foo";
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = "foo";
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
