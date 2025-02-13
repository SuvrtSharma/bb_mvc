using System;
using System.Collections.Generic;

namespace bb.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }   // ID of the customer who placed the order
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }

        // Navigation property – list of items in this order
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
