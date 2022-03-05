
using Microsoft.EntityFrameworkCore;

namespace test_api;

public class BullionContext : DbContext
{
    public BullionContext(DbContextOptions<BullionContext> options) : base(options)
    {
    }
    public DbSet<Bullion> Bullions { get; set; }
    public DbSet<Basket> Baskets { get; set; }

}
