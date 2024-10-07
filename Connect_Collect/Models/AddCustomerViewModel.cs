using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models
{
    public class AddCustomerViewModel
    {
        [Key]
        public Guid CustomerId { get; set; }

        [MaxLength(100)]
        public required string CustomerName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
    }
}
