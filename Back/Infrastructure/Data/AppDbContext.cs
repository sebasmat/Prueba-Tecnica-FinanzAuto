
using Microsoft.EntityFrameworkCore;
using Back.Core.Entities; 

namespace Back.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

}