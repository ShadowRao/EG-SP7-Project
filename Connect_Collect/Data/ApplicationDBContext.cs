using System.Collections.Generic;
using Connect_Collect.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Connect_Collect.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<Seller> Seller { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Cart> Cart { get; set; }

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
        }
    }
}
