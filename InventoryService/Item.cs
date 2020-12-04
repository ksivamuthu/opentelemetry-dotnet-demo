using System;

namespace InventoryService
{
    public class Item
    {
        public int Id { get; set; }
        
        public string ItemCode { get; set; }

        public int TotalQuantity { get; set; }
    }

    public class ItemVerification 
    {
        public string ItemCode { get; set; }

        public int Quantity { get; set; }
    }

    public class ItemClaim
    {
        public string ItemCode { get; set; }

        public int Quantity { get; set; }
    }
}
