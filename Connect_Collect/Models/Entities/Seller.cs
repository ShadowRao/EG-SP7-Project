using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models.Entities
{
    public class Seller
    {
        [Key]
        public Guid SellerId { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [MaxLength(100)]
        public string? SellerName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public required string Password { get; set; }
        [Required(ErrorMessage ="Contact number is required")]
        public string? Contact { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
