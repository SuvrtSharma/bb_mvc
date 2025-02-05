using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bb.Models
{
    public class Cart
    {
       
            [Key]
            public int Id { get; set; }

            [Required]
            public string UserId { get; set; } // Foreign Key for User  

            [Required]
            public int ProductId { get; set; } // Foreign Key for Product  

            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
            public int Quantity { get; set; }

            [ForeignKey("ProductId")]
            public Product Product { get; set; }

            [ForeignKey("UserId")]
            public ApplicationUser User { get; set; }
        }
    }

