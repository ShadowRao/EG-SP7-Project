using System.ComponentModel.DataAnnotations;
using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connect_Collect.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDBContext dBContext;

        public CustomerController(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpGet]
        public IActionResult CustomerSignup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomerSignup(CustomerSignupViewModel viewModel)
        {
            //Adding data to a variable
            var customer = new Customer  
            {
                CustomerName = viewModel.CustomerName,
                Email = viewModel.Email,
                Address = viewModel.Address,
                Password = viewModel.Password,

            };
            await dBContext.Customer.AddAsync(customer); //Adding to DB
            await dBContext.SaveChangesAsync(); //Saving to DB

            return View();
        }
    }
}
