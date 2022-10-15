using GroceryMaster.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryMaster.Data;
public class GroceryMasterDbContext: DbContext
{
#nullable disable
    public DbSet<Aisle> Aisles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Store> Stores { get; set; }
#nullable restore

    public GroceryMasterDbContext()   { }

    public GroceryMasterDbContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


}
