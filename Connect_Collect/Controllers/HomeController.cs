using System.Diagnostics;
using System.Security.Claims;
using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                // Return JSON response if already logged in and it's an API request
                return Json(new
                {
                    success = true,
                    message = "Redirects to Home page"
                });
            }
            else { 
            
            return View();
            }


        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == true)
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    // Return JSON response if already logged in and it's an API request
                    return Json(new
                    {
                        success = false,
                        message = "You are already logged in."
                    });
                }
                else
                {
                    return View("AlreadyLoggedIn");
                }
            }
                return View();
        }


        [AllowAnonymous]
        [HttpPost]
        //Verifies login for customer while signing in
        public async Task<IActionResult> SignIn( SignInModel model)
        {
            var isAuthenticated = User.Identity.IsAuthenticated;

            /*if (!ModelState.IsValid)
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    // Return JSON response if already logged in and it's an API request
                    return Json(new
                    {
                        success = false,
                        message = "Invalid"
                    });
                }
                else
                {
                    return View(model);
                }
            }*/

            if (isAuthenticated == true)
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    // Return JSON response if already logged in and it's an API request
                    return Json(new
                    {
                        success = false,
                        message = "Login accepted and redirected to dashboard"
                    });
                }
                else
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Privacy", "Home");
                }
            }

            else
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {

                    using (var reader = new StreamReader(Request.Body))
                    {
                        var body = await reader.ReadToEndAsync();
                        var viewModel = JsonConvert.DeserializeObject<SignInModel>(body);

                        // Verify email is valid of Customer
                        var CUser = dbContext.Customer.FirstOrDefault(u => u.Email == viewModel.Email);

                        // Verify email is valid of Seller
                        var SUser = dbContext.Seller.FirstOrDefault(u => u.Email == viewModel.Email);

                        // Verify email is valid of Admin
                        var AUser = dbContext.Admin.FirstOrDefault(u => u.Email == viewModel.Email);


                        if (CUser != null)
                        {
                            var passwordHasher = new PasswordHasher<Customer>();
                            var result = passwordHasher.VerifyHashedPassword(CUser, CUser.Password, viewModel.Password);



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

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                Console.WriteLine("Customer logged in successfully.");
                                return Json(new
                                {
                                    success = true,
                                    message = "Customer user logged in"
                                });
                            }
                        }

                        if (SUser != null)
                        {
                            var passwordHasher = new PasswordHasher<Seller>();
                            var result = passwordHasher.VerifyHashedPassword(SUser, SUser.Password, viewModel.Password);

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

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                Console.WriteLine("Seller logged in successfully.");
                                return Json(new
                                {
                                    success = true,
                                    message = "Seller logged in"
                                });
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

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            // Redirect to the desired page after successful login
                            return Json(new
                            {
                                success = true,
                                message = "Admin logged in"
                            });
                        }
                    }
                }

                else
                {
                    // Verify email is valid of Customer
                    var CUser = dbContext.Customer.FirstOrDefault(u => u.Email == model.Email);

                    // Verify email is valid of Seller
                    var SUser = dbContext.Seller.FirstOrDefault(u => u.Email == model.Email);

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

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        // Redirect to the desired page after successful login
                        return RedirectToAction("Home", "Admin");
                    }
                }
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    // Return JSON response if already logged in and it's an API request
                    return Json(new
                    {
                        success = false,
                        message = "Invalid email or password"
                    });
                }

                // Add an error message to the model state if login fails
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Json(new
                {
                    success = true,
                    message = "Logged out"
                });
            }

            else
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("SignIn", "Home");
            }
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                // Return JSON response if already logged in and it's an API request
                return Json(new
                {
                    success = false,
                    message = "Redirects to Privacy page"
                });
            }
            else
            {
            return View();

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
