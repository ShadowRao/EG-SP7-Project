namespace Connect_Collect.Models.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid SellerId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
       // public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation properties
        public Seller Seller { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
