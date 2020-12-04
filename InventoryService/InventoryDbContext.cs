using System;
using Microsoft.EntityFrameworkCore;

namespace InventoryService
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
            DbInitializer.Initialize(this);
        }

        public DbSet<Item> Items {get;set;}
    }
}
