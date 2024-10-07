using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models
{
    public class AddSellerViewModel
    {
        public Guid SellerId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string? SellerName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        public string? Contact { get; set; }
    }
}
