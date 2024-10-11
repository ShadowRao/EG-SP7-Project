using System.Diagnostics;
using System.Security.Claims;
using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                
            // Verify email is valid of Customer
            var CUser = dbContext.Customer.FirstOrDefault(u => u.Email == model.Email);

            // Verify email is valid of Seller
            var SUser = dbContext.Seller.FirstOrDefault(u => u.Email == model.Email );

            // Verify email is valid of Admin
            var AUser = dbContext.Admin.FirstOrDefault(u => u.Email == model.Email);


            if (CUser != null)
            {
                var passwordHasher = new PasswordHasher<Customer>();
                var result = passwordHasher.VerifyHashedPassword(CUser, CUser.Password, model.Password);

                

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, CUser.Email),
                        new Claim(ClaimTypes.Role, "Customer"),
                        new Claim("CustomerId", CUser.CustomerId.ToString()) // Store CustomerId as a claim
                    };
                    var identity = new ClaimsIdentity(claims, "Customer");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                    Console.WriteLine("Customer logged in successfully.");
                    return RedirectToAction("Home", "Customer");
                }
            }

            if (SUser != null)
            {
                var passwordHasher = new PasswordHasher<Seller>();
                var result = passwordHasher.VerifyHashedPassword(SUser, SUser.Password, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, SUser.Email),
                        new Claim(ClaimTypes.Role, "Seller"),
                        new Claim("SellerId", SUser.SellerId.ToString()) // Store CustomerId as a claim
                    };
                    var identity = new ClaimsIdentity(claims, "Seller");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                    Console.WriteLine("Seller logged in successfully.");
                    return RedirectToAction("Home", "Seller");
                }
                
            }

            if (AUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, AUser.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("AdminId", AUser.AdminId.ToString()) // Store CustomerId as a claim
                };
                var identity = new ClaimsIdentity(claims, "Admin");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                // Redirect to the desired page after successful login
                return RedirectToAction("Home", "Admin"); 
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
