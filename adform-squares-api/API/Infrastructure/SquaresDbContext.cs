using Microsoft.EntityFrameworkCore;
using API.Core.Entities;

namespace API.Infrastructure;

public class SquaresDbContext : DbContext
{
    public DbSet<UserDocument> Users { get; set; }
    public DbSet<PointDocument> Points { get; set; }

    public SquaresDbContext(DbContextOptions<SquaresDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PointDocument>()
            .HasIndex(p => new { p.UserId, p.X, p.Y })
            .IsUnique();

        modelBuilder.Entity<UserDocument>()
            .HasIndex(u => u.Name)
            .IsUnique();
    }
}
