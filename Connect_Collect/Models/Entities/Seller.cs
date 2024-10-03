using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models.Entities
{
    public class Seller
    {
        [Key]
        public Guid SellerId { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [MaxLength(100)]
        public required string SellerName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public required string email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public required string passsword { get; set; }
        [Required(ErrorMessage ="Contact number is required")]
        public required string Contact { get; set; }  
        
    }
}
