using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models.Entities
{
    public class Admin
    {
        public Guid AdminId { get; set; }

        [MaxLength(100)]
        public string? AdminName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
