using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace bb.Models
{
    public class ProductSubmission
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        // Although the Product model has an Author field,
        // you can auto-populate this from the logged-in user.
        public string Author { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }

        // File uploads:
        [Display(Name = "Book Image")]
        public IFormFile Image { get; set; }

        [Display(Name = "Preview File (PDF)")]
        public IFormFile PreviewFile { get; set; }

        [Display(Name = "E-book File (PDF)")]
        public IFormFile EBookFile { get; set; }
    }
}