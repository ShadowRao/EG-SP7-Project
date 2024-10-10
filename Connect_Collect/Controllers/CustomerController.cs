using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Connect_Collect.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public CustomerController(ApplicationDbContext dbContext)
        {
            this.dbContext=dbContext;
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
            var customerdata = await dbContext.Customer.FindAsync(Id);
            return View(customerdata);
        }

        /* public IActionResult Add(AddCustomerViewModel viewModel)
         {
             return View();
         }
         [HttpGet]*/
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

            return View();
        }


        public IActionResult ViewProducts()
        {
            var products = dbContext.Product;

            // Manually map to ProductViewModel
            var productViewModels = products.Select(product => new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SellerId = product.SellerId,
            });

            // Return the view with the mapped ProductViewModel collection
            return View(productViewModels.ToList());
        }
    }
}
