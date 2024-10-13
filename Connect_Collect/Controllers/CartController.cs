using Connect_Collect.Data;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Connect_Collect.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(Guid productId)
        {
            // Hardcoded customer ID for testing
            var customerId = User.FindFirst("CustomerId")?.Value;

            // Check if the product is already in the cart for the given customer
            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(c => c.CustomerId.ToString() == customerId && c.ProductId == productId);

            if (cartItem != null)
            {
                // If the product already exists in the cart, update the quantity
                cartItem.Quantity += 1;
            }
            else
            {
                // If the product is not in the cart, create a new cart entry
                cartItem = new Cart
                {
                    //CustomerId = customerId,
                    ProductId = productId,
                    Quantity = 1 // Set initial quantity to 1
                };

                _context.Cart.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // Redirect to the Cart Contents page
            return RedirectToAction("CartContents");
        }


        [HttpGet]
        public async Task<IActionResult> CartContents()
        {
            // Hardcoded customer ID for testing
            var customerId = User.FindFirst("CustomerId")?.Value;

            // Get cart items for the specific customer
            var cartItems = await _context.Cart
                .Include(c => c.Product) // Include related product data
                .Where(c => c.CustomerId == Guid.Parse(customerId))
                .ToListAsync();

            return View(cartItems); // Pass the cart items to the view
        }
    }
}
