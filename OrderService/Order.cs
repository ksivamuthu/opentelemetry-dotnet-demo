using System;

namespace OrderService
{
    public class Order
    {
        public int Id { get; set; }
        
        public string ItemCode { get; set; }

        public int Quantity { get; set; }

        public string Username { get; set; }
    }
}
