using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService
{
    public class DbInitializer
    {
        public static void Initialize(InventoryDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Items.Any())
            {                
                return;
            }
        
            var items = new List<Item>() { 
                new Item { ItemCode = "PIZZA", TotalQuantity = 15 },
                new Item { ItemCode = "COFFEE", TotalQuantity = 15 },
                new Item { ItemCode = "COKE", TotalQuantity = 15 },                                
                new Item { ItemCode = "TEA", TotalQuantity = 15 },                                                
                new Item { ItemCode = "SANDWICH", TotalQuantity = 15 },                                                
            };

            context.Items.AddRange(items);
            context.SaveChanges();
        }
    }
}