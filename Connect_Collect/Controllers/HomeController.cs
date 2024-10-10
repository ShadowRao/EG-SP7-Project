using System.Diagnostics;
using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect_Collect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpGet]
        //Verifies login for customer while signing in
        public IActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                
            // Verify the email and password of a customer
            var CUser = dbContext.Customer.FirstOrDefault(u => u.Email == model.Email); // Make sure to hash passwords in real applications

            // Verify the email and password of a Seller
            var SUser= dbContext.Seller.FirstOrDefault(u => u.Email == model.Email );

            //Verify the email and password of a Admin
            var AUser = dbContext.Admin.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (CUser != null)
            {
                var passwordHasher = new PasswordHasher<Customer>();
                var result = passwordHasher.VerifyHashedPassword(CUser, CUser.Password, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    Console.WriteLine("Customer logged in successfully.");
                    return RedirectToAction("Home", "Customer", new { Id = CUser.CustomerId });
                }
                

            }

            if (SUser != null)
            {
                var passwordHasher = new PasswordHasher<Seller>();
                var result = passwordHasher.VerifyHashedPassword(SUser, SUser.Password, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    Console.WriteLine("Seller logged in successfully.");
                    return RedirectToAction("Home", "Seller", new { Id = SUser.SellerId });
                }
                
            }




            if (AUser != null)
            {
                // Redirect to the desired page after successful login
                return RedirectToAction("Home", "Admin", new { Id = AUser.AdminId }); // Adjust as necessary
            }

            // Add an error message to the model state if login fails
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
