using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models.Entities
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        [MaxLength(100)]
        public string? CustomerName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public  string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set; }
        public string? Address { get; set; }

        public ICollection<Cart>? Cart { get; set; }

    }
}
