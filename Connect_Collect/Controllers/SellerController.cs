using Connect_Collect.Data;
using Connect_Collect.Models.Entities;
using Connect_Collect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace Connect_Collect.Controllers
{
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SellerController(ApplicationDbContext dbContext)
        {
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
        [HttpGet]
        public IActionResult AddSeller()
        {
            return View();
        }
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
                // Hash the password before saving it to the database
                Password = passwordHasher.HashPassword(null, viewModel.Password),
                Contact = viewModel.Contact,
            };

            await dbContext.Seller.AddAsync(seller);
            await dbContext.SaveChangesAsync();

            ViewBag.SuccessMessage = "Successfully signed up. Please login.";

            return RedirectToAction("SignIn", "Home");
        }

        [HttpGet]
        public IActionResult AddProduct(Guid id)
        {
            // Populate the seller list for the dropdown
            ViewBag.SellerList = new SelectList(dbContext.Seller, "SellerId", "SellerName");

            ViewBag.SellerId = id;
            var seller = dbContext.Seller
                .FirstOrDefault(s => s.SellerId == id);
            return View();
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

                return RedirectToAction("AddProduct");

            }

            return View(model);  // Handle case when no file is uploaded
        }

        //Viewing orders from the seller dashboard
        [HttpGet]
        public async Task<IActionResult> ViewOrders(Guid id)
        {
            // Fetch all orders related to the seller by SellerId
            var orders = await dbContext.Order
                .Where(o => o.SellerId == id)
                .Include(o => o.Customer) // Optionally include related customer data
                .ToListAsync();

            // Pass the list of orders to the view
            return View(orders);
        }




    }
}
