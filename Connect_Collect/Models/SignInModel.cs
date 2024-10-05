using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models
{
    public class SignInModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
