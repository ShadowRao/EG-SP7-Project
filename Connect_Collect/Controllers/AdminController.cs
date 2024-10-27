using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Connect_Collect.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp()
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                // Return JSON response if already logged in and it's an API request
                return Json(new
                {
                    success = true,
                    message = "Redirects to Admin SIgnup"
                });
            }
            else
            {
                return View();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(AdminViewModel viewModel)
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    var ViewModel = JsonConvert.DeserializeObject<AdminViewModel>(body);


                    // Create an instance of PasswordHasher to hash the password
                    var passwordHasher = new PasswordHasher<Admin>();

                    var admin = new Admin
                    {
                        AdminName = ViewModel.AdminName,
                        AdminId = ViewModel.AdminId,
                        Email = ViewModel.Email,
                        // Hash the password before saving it to the database
                        Password = passwordHasher.HashPassword(null, ViewModel.Password),

                    };

                    await dbContext.Admin.AddAsync(admin);
                    await dbContext.SaveChangesAsync();

                    return Json(new
                    {
                        success = true,
                        message = "Admin signed up"
                    });
                }
            }

            else
            {
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
        }


        [HttpGet]
        public async Task<IActionResult> Home()
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                var adminIdClaim = User.FindFirst("AdminId")?.Value;

                if (adminIdClaim == null)
                {
                    // Handle the case where AdminId is not found in claims
                    return RedirectToAction("SignIn", "Home"); // Redirect to sign in page or an error page
                }
                var adminId = Guid.Parse(adminIdClaim); // Parse the AdminId
                var admindata = await dbContext.Admin.FindAsync(adminId);

                return Json(new
                {
                    success = true,
                    
                    message = new
                    {
                        admindata.Email, admindata.AdminName, admindata.AdminId
                    }
                });
            }
            else
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
        }


        [HttpGet]
        public async Task<IActionResult> CustomerList()
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                var customer = await dbContext.Customer.Select(c => new {c.CustomerId,c.CustomerName,c.Cart,c.Address,c.Email,c.Contact}).ToListAsync();
                return Json(new
                {
                    success = false,
                    message = customer
                });
            }
            else
            {
                var customer = await dbContext.Customer.ToListAsync();
                return View(customer);
            }
        }
        [HttpGet]
        public async Task<IActionResult> CustomerEdit(Guid id)
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                var customer = await dbContext.Customer.FindAsync(id);
                return Json(new
                {
                    success = false,
                    message = new
                    {
                        customer.CustomerName,
                        customer.CustomerId,
                        customer.Address,
                        customer.Email,
                        customer.Contact,
                        customer.Cart
                    }
                });
            }
            else
            {
                var customer = await dbContext.Customer.FindAsync(id);
                return View(customer);
            }
        }
        [HttpPost,HttpPut]
        public async Task<IActionResult> CustomerEdit(Customer viewModel)
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    var ViewModel = JsonConvert.DeserializeObject<CustomerEditModel>(body);

                    var customer = await dbContext.Customer.FindAsync(ViewModel.CustomerId);
                    if (customer is not null)
                    {
                        customer.CustomerName = ViewModel.CustomerName;
                        customer.CustomerId = ViewModel.CustomerId;
                        customer.Email = ViewModel.Email;
                        customer.Address = ViewModel.Address;

                        await dbContext.SaveChangesAsync();

                    }
                    return Json(new
                    {
                        success = false,
                        message = customer
                    });
                }
            }

            else
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
        }
        [HttpPost]
        public async Task<IActionResult> CustomerDelete(Customer viewModel)
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
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

            else
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
        }


        [HttpGet]
        public async Task<IActionResult> SellerList()
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                var seller = await dbContext.Seller.Select(s => new { s.SellerId,s.SellerName,s.Email,s.Orders}).ToListAsync();
                return Json(new
                {
                    success = true,
                    message = seller
                });
            }
            else 
            {
                var seller = await dbContext.Seller.ToListAsync();
                return View(seller);
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> SellerEdit(Guid id)
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                var seller = await dbContext.Seller.FindAsync(id);
                return Json(new
                {
                    success = false,
                    message = seller
                });
            }
            else
            {
                var seller = await dbContext.Seller.FindAsync(id);
                return View(seller);
            }    
            
        }
        [HttpPost]
        public async Task<IActionResult> SellerEdit(Seller viewModel)
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    var ViewModel = JsonConvert.DeserializeObject<Seller>(body);

                    var seller = await dbContext.Seller.FindAsync(ViewModel.SellerId);
                    if (seller is not null)
                    {
                        seller.SellerName = ViewModel.SellerName;
                        seller.SellerId = ViewModel.SellerId;
                        seller.Email = ViewModel.Email;
                        seller.Password = ViewModel.Password;
                        seller.Contact = ViewModel.Contact;

                        await dbContext.SaveChangesAsync();

                    }
                    var sellerlist = await dbContext.Seller.ToListAsync();

                    return Json(new
                    {
                        success = false,
                        message = sellerlist
                    });
                }
            }
            else
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
        }
        [HttpPost]
        public async Task<IActionResult> SellerDelete(Seller viewModel)
        {
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    var ViewModel = JsonConvert.DeserializeObject<Seller>(body);

                    var seller = await dbContext.Seller
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.SellerId == ViewModel.SellerId);
                    if (seller is not null)
                    {
                        dbContext.Seller.Remove(ViewModel);
                        await dbContext.SaveChangesAsync();
                    }
                    var sellerlist = await dbContext.Seller.ToListAsync();

                    return Json(new
                    {
                        success = false,
                        message = sellerlist
                    });
                }
            }
            else
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
}
