﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect_Collect.Models.Entities
{
    public class Product
    {

        [Key]
        public Guid ProductId { get; set; }

        public required string ProductName { get; set; }
        [ForeignKey("Seller")]
        public Guid SellerId { get; set; }
        
        public required string ProductDescription { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }  // IFormFile is ignored by EF

        public string? ImageUrl { get; set; }  // Store image URL or file path

        public int Price { get; set; }
        public ICollection<Cart>? Cart { get; set; }

        public virtual Seller? Seller { get; set; }

    }
}
