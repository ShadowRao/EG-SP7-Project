using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models
{
    public class CustomerSignupViewModel
    {
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
    }
}
