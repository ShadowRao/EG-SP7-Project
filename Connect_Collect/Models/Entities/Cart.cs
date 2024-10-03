using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect_Collect.Models.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? ProductId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }

    }
}
