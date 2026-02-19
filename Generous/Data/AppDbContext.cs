using Microsoft.EntityFrameworkCore;
using Generous.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Add your DbSets here
    public DbSet<Element> Elements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This moves everything in this context to the "app" schema
        modelBuilder.HasDefaultSchema("app");

        base.OnModelCreating(modelBuilder);
    }
}