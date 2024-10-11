using Connect_Collect.Data; // Make sure to include this namespace for ApplicationDbContext
using Connect_Collect.Models.Entities; // Include your Product entity
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Connect_Collect.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject ApplicationDbContext
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid productId)
        {
            // Retrieve the product by ID
            var product = await _context.Product
                .Include(p => p.Seller) // Optionally include related seller data if you want to display seller info
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound(); // Return a 404 error if the product is not found
            }

            return View(product); // Pass the product to the Details view
        }
    }
}
