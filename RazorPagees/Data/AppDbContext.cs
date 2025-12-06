using Microsoft.EntityFrameworkCore;
using RazorPagees.Domain;

namespace RazorPagees.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppEntity> Apps => Set<AppEntity>();

    public DbSet<AlternativeEntity> Alternatives => Set<AlternativeEntity>();

    public DbSet<AppTag> Tags => Set<AppTag>();

    public DbSet<VoteRecord> Votes => Set<VoteRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppEntity>()
            .HasIndex(a => a.Slug)
            .IsUnique();

        modelBuilder.Entity<AppEntity>()
            .Property(a => a.Name)
            .HasMaxLength(160);

        modelBuilder.Entity<AppEntity>()
            .Property(a => a.Category)
            .HasMaxLength(120);

        modelBuilder.Entity<AppEntity>()
            .Property(a => a.Views)
            .HasDefaultValue(0);

        modelBuilder.Entity<AppTag>()
            .HasOne(t => t.App)
            .WithMany(a => a.Tags)
            .HasForeignKey(t => t.AppId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppTag>()
            .HasIndex(t => new { t.AppId, t.Value })
            .IsUnique();

        modelBuilder.Entity<AlternativeEntity>()
            .HasOne(a => a.App)
            .WithMany(a => a.Alternatives)
            .HasForeignKey(a => a.AppId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AlternativeEntity>()
            .HasIndex(a => new { a.AppId, a.Name })
            .IsUnique();

        modelBuilder.Entity<VoteRecord>()
            .HasIndex(v => new { v.AppSlug, v.Direction });

        base.OnModelCreating(modelBuilder);
    }
}
