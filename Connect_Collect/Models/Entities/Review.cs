using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect_Collect.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        [ForeignKey("CustomerUsername")]
        public string? CustomerUsername { get; set; }

        public string? Comment { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        public int Rating { get; set; }

  
       
        public virtual Product? Product { get; set; } // Reference to the Product

       
        public virtual Customer? Customer { get; set; } // Reference to the Customer
    }
}
