using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect_Collect.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AdminViewModel viewModel)
        {
            // Create an instance of PasswordHasher to hash the password
            var passwordHasher = new PasswordHasher<Admin>();

            var admin = new Admin
            {
                AdminName = viewModel.AdminName,
                AdminId = viewModel.AdminId,
                Email = viewModel.Email,
                // Hash the password before saving it to the database
                Password = passwordHasher.HashPassword(null, viewModel.Password),
                
            };

            await dbContext.Admin.AddAsync(admin);
            await dbContext.SaveChangesAsync();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var adminIdClaim = User.FindFirst("AdminId")?.Value;

            if (adminIdClaim == null)
            {
                // Handle the case where AdminId is not found in claims
                return RedirectToAction("SignIn", "Home"); // Redirect to sign in page or an error page
            }
            var adminId = Guid.Parse(adminIdClaim); // Parse the AdminId
            var admindata = await dbContext.Admin.FindAsync(adminId);
            //var admindata = await dbContext.Admin.FindAsync(Id);
            return View(admindata);
        }


        [HttpGet]
        public async Task<IActionResult> CustomerList()
        {
            var customer = await dbContext.Customer.ToListAsync();
            return View(customer);
        }
        [HttpGet]
        public async Task<IActionResult> CustomerEdit(Guid id)
        {
            var customer = await dbContext.Customer.FindAsync(id);
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> CustomerEdit(Customer viewModel)
        {
            var customer = await dbContext.Customer.FindAsync(viewModel.CustomerId);
            if (customer is not null)
            {
                customer.CustomerName = viewModel.CustomerName;
                customer.CustomerId = viewModel.CustomerId;
                customer.Email = viewModel.Email;
                customer.Password = viewModel.Password;
                customer.Address = viewModel.Address;

                await dbContext.SaveChangesAsync();

            }
            return RedirectToAction("CustomerList", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> CustomerDelete(Customer viewModel)
        {
            var customer = await dbContext.Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CustomerId == viewModel.CustomerId);
            if (customer is not null)
            {
                dbContext.Customer.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("CustomerList", "Admin");
        }


        [HttpGet]
        public async Task<IActionResult> SellerList()
        {
            var seller = await dbContext.Seller.ToListAsync();
            return View(seller);
        }
        [HttpGet]
        public async Task<IActionResult> SellerEdit(Guid id)
        {
            var seller = await dbContext.Seller.FindAsync(id);
            return View(seller);
        }
        [HttpPost]
        public async Task<IActionResult> SellerEdit(Seller viewModel)
        {
            var seller = await dbContext.Seller.FindAsync(viewModel.SellerId);
            if (seller is not null)
            {
                seller.SellerName = viewModel.SellerName;
                seller.SellerId = viewModel.SellerId;
                seller.Email = viewModel.Email;
                seller.Password = viewModel.Password;
                seller.Contact = viewModel.Contact;

                await dbContext.SaveChangesAsync();

            }
            return RedirectToAction("SellerList", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> SellerDelete(Seller viewModel)
        {
            var seller = await dbContext.Seller
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SellerId == viewModel.SellerId);
            if (seller is not null)
            {
                dbContext.Seller.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("SellerList", "Admin");
        }
    }
}
