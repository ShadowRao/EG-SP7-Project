using System.Diagnostics;
using Connect_Collect.Data;
using Connect_Collect.Models;
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
                
            // Verify the email and password
            var user = dbContext.Customer
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password); // Make sure to hash passwords in real applications

            if (user != null)
            {
                // Redirect to the desired page after successful login
                return RedirectToAction("Home", "Customer", new { Id = user.CustomerId }); // Adjust as necessary
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
