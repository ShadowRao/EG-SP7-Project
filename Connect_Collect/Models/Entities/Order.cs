using System;
using System.ComponentModel.DataAnnotations;

namespace Connect_Collect.Models.Entities
{
    public class Order
    {
        // Remove the [Key] attribute to allow for custom key configurations if needed
        public Guid OrderId { get; set; } // Primary key for the Order

        public Guid SellerId { get; set; } // Foreign key referencing the Seller
        public Guid CustomerId { get; set; } // Foreign key referencing the Customer
        public Guid ProductId { get; set; } // Foreign key referencing the Product
        // Uncomment this line if you want to track the total price of the order
        // public decimal TotalPrice { get; set; } 

        public DateTime OrderDate { get; set; } // Date the order was placed

        // Navigation properties to related entities
        public virtual Seller Seller { get; set; } // Navigation property for Seller
        public virtual Customer Customer { get; set; } // Navigation property for Customer
        public virtual Product Product { get; set; } // Navigation property for Product
    }
}
