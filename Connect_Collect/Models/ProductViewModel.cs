using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }

        public required string ProductName { get; set; }
        public Guid SellerId { get; set; }

        public required string ProductDescription { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }  // IFormFile is ignored by EF

        public string? ImageUrl { get; set; }  // Store image URL or file path

        public int Price { get; set; }
    }
}
