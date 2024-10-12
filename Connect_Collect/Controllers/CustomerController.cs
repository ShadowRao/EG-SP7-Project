using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Connect_Collect.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CustomerController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public object Finish { get; private set; }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Home(Guid Id)
        {
            var CustomerIdClaim = User.FindFirst("CustomerId")?.Value;

            if (CustomerIdClaim == null)
            {
                // Handle the case where AdminId is not found in claims
                return RedirectToAction("SignIn", "Home"); // Redirect to sign in page or an error page
            }
            var customerId = Guid.Parse(CustomerIdClaim); // Parse the AdminId
            var customerdata = await dbContext.Customer.FindAsync(customerId);

            //var customerdata = await dbContext.Customer.FindAsync(Id);
            return View(customerdata);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerViewModel viewModel)
        {
            // Create an instance of PasswordHasher to hash the password
            var passwordHasher = new PasswordHasher<Customer>();

            var customer = new Customer
            {
                CustomerName = viewModel.CustomerName,
                CustomerId = viewModel.CustomerId,
                Email = viewModel.Email,
                // Hash the password before saving it to the database
                Password = passwordHasher.HashPassword(null, viewModel.Password),
                Address = viewModel.Address,
                Contact = viewModel.Contact,
            };

            await dbContext.Customer.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            ViewBag.SuccessMessage = "Successfully signed up. Please login.";

            return RedirectToAction("SignIn", "Home");
        }

        public IActionResult ViewProducts()
        {
            // Fetch products and include Seller data
            var products = dbContext.Product
                .Include(p => p.Seller); // Include related Seller data

            // Manually map to ProductViewModel
            var productViewModels = products.Select(product => new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SellerId = product.SellerId,
                Seller = product.Seller // Include Seller information in the ViewModel
            });

            // Return the view with the mapped ProductViewModel collection
            return View(productViewModels.ToList());
        }
    }
}
