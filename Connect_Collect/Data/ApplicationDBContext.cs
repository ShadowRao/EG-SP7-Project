 using System.Collections.Generic;
using Connect_Collect.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Connect_Collect.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<Seller> Seller { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Cart> Cart { get; set; }

/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany() // You may want to configure this depending on your design
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust this if needed

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany() // Configure navigation property if needed
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes here
        }*/
    }
}
