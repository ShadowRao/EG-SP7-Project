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

        public DbSet<Review> Review { get; set; }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<Order> Order { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Enforce a unique constraint on the Email column
            modelBuilder.Entity<Customer>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Seller>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Cart>()
            .HasKey(c => new { c.CustomerId, c.ProductId }); // Configure composite key
            modelBuilder.Entity<Cart>()
            .HasOne(c => c.Customer)
            .WithMany(c => c.Cart) // Assuming a customer can have many carts
            .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior

            modelBuilder.Entity<Cart>()
            .HasOne(c => c.Product)
            .WithMany(c => c.Cart) // Assuming a product can be in many carts
            .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior
        }
    }
}
