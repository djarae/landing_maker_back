using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LandingMakerBack.Models;

namespace LandingMakerBack.Data;

/// <summary>
/// Contexto de base de datos principal
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Landing> Landings { get; set; }
    public DbSet<LandingSection> LandingSections { get; set; }
    public DbSet<UploadedImage> UploadedImages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Landing configuration
        builder.Entity<Landing>(entity =>
        {
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsPublished);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Landings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // LandingSection configuration
        builder.Entity<LandingSection>(entity =>
        {
            entity.HasIndex(e => new { e.LandingId, e.Order });
            
            entity.HasOne(e => e.Landing)
                .WithMany(l => l.Sections)
                .HasForeignKey(e => e.LandingId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // UploadedImage configuration
        builder.Entity<UploadedImage>(entity =>
        {
            entity.HasIndex(e => e.UserId);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
