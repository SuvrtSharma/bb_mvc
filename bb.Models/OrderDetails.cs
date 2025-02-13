using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bb.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }     // Foreign key to Order
        public int ProductId { get; set; }   // ID of the product ordered
        public int Quantity { get; set; }
        public decimal Price { get; set; }   // Price for this line item (e.g., unit price * quantity)

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
