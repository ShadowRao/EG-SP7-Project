using Connect_Collect.Data;
using Connect_Collect.Models.Entities;
using Connect_Collect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace Connect_Collect.Controllers
{
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly EmailService _emailService;

        public SellerController(ApplicationDbContext dbContext,EmailService emailService)
        {
            
            _emailService = emailService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Home(Guid Id)
        {
            var SellerIdClaim = User.FindFirst("SellerId")?.Value;

            if (SellerIdClaim == null)
            {
                // Handle the case where AdminId is not found in claims
                return RedirectToAction("SignIn", "Home"); // Redirect to sign in page or an error page
            }
            var sellerId = Guid.Parse(SellerIdClaim); // Parse the AdminId
            var sellerdata = await dbContext.Seller.FindAsync(sellerId);
            //var sellerdata = await dbContext.Seller.FindAsync(Id);
            return View(sellerdata);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AddSeller()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddSeller(AddSellerViewModel viewModel)
        {
            // Create an instance of PasswordHasher to hash the password
            var passwordHasher = new PasswordHasher<Seller>();

            var seller = new Seller
            {
                SellerName = viewModel.SellerName,
                SellerId = viewModel.SellerId,
                Email = viewModel.Email,
                // Hashing the password before saving it to the database
                Password = passwordHasher.HashPassword(null, viewModel.Password),
                Contact = viewModel.Contact,
            };

            await dbContext.Seller.AddAsync(seller);
            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Successfully signed up. Please login.";

            //return View();
            return RedirectToAction("AddSeller");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var sellerIdClaim = User.FindFirst("SellerId")?.Value;
            var sellerId = Guid.Parse(sellerIdClaim);
            // Populate the seller list for the dropdown
            ViewBag.SellerList = new SelectList(dbContext.Seller, "SellerId", "SellerName");
            ViewBag.SellerId = sellerId;

            return View();
            //return RedirectToAction("AddProduct");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (model.ImageFile != null)
            {
                // Get the path to save the image
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

                // Create a unique filename for the image
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName); // Keep the file extension
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image to the file system
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                // Save only the image URL to the database
                model.ImageUrl = "/images/" + uniqueFileName;
                var product = new Product
                {
                    ProductId = model.ProductId,
                    ProductName = model.ProductName,
                    SellerId = model.SellerId,
                    ProductDescription = model.ProductDescription,
                    ImageUrl = model.ImageUrl,
                    Price = model.Price,
                };
                // Add the model to the database
                dbContext.Product.Add(product);
                await dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product added successfully!";

                return RedirectToAction("AddProduct");
            }

            return View(model);  // Handle case when no file is uploaded
        }

        [HttpGet]
        public async Task<IActionResult> ViewOrders()
        {
            var sellerId = User.FindFirst("SellerId")?.Value;
            // Fetch all cart items related to the products that belong to the seller
            var orders = await dbContext.Cart
                .Include(c => c.Customer)  // Include related customer data
                .Include(c => c.Product)   // Include related product data
                .Where(c => c.Product.SellerId.ToString() == sellerId)  // Filter by the seller's products
                .ToListAsync();

            if (!orders.Any())
            {
                Console.WriteLine("No orders found for the seller");
            }

            return View(orders);  // Pass the cart items (orders) to the view
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string customerEmail, Guid productId)
        {
            var productdata=await dbContext.Product.FindAsync(productId);
            var sellerid = User.FindFirst("SellerId")?.Value;
            var sellerdata = await dbContext.Seller.FindAsync(Guid.Parse(sellerid));
            await _emailService.SendEmailAsync(customerEmail,"Hey "+ productdata.ProductName.ToString() +" is available" , "The product  "+ productdata.ProductName.ToString()+ " is ready to be sold by " + sellerdata.SellerName.ToString()+" \n You can contact them through "+sellerdata.Email.ToString());

            // Redirect back to the ViewOrders page
            return RedirectToAction("ViewOrders");
        }



    }
}
