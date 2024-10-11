using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Mvc;
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
            var customerdata = await dbContext.Customer.FindAsync(Id);
            return View(customerdata);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerViewModel viewModel)
        {
            var customer = new Customer
            {
                CustomerName = viewModel.CustomerName,
                CustomerId = viewModel.CustomerId,
                Email = viewModel.Email,
                Password = viewModel.Password,
                Address = viewModel.Address,
                //Finish=viewModel.Finish
            };

            await dbContext.Customer.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            return View();
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
