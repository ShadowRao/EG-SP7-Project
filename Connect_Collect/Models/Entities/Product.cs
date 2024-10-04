using System.ComponentModel.DataAnnotations;
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
        
        public byte[]? Image { get; set; }
        public int Price { get; set; }
        public virtual Seller? Seller { get; set; }

    }
}
