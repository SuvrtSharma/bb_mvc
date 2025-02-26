using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace bb.Models
{
    // Add an enum for the approval workflow
    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
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

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [ValidateNever]
        [Display(Name = "Preview URL")]
        public string? PreviewUrl { get; set; }

        [Display(Name = "E-book URL")]
        public string? EBook { get; set; }

        // --- New fields for submission workflow ---
        // For admin-entered products, leave the default as Approved.
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Approved;

        // For author submissions, record who submitted and when.
        public string? SubmittedBy { get; set; }
        public DateTime? SubmittedOn { get; set; }
    }
}