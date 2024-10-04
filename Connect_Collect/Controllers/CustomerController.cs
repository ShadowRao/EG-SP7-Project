using Connect_Collect.Data;
using Connect_Collect.Models;
using Connect_Collect.Models.Entities;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
       /* public IActionResult Add(AddCustomerViewModel viewModel)
        {
            return View();
        }
        [HttpGet]*/
        public async Task<IActionResult> AddCustomer(AddCustomerViewModel viewModel)
        {
            var customer = new Customer
            {
                CustomerName = viewModel.CustomerName,
                CustomerId = viewModel.CustomerId,
                Email=viewModel.Email,
                Password=viewModel.Password,
                Address=viewModel.Address,
                //Finish=viewModel.Finish
            };
            await dbContext.Customer.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var customer=await dbContext.Customer.ToListAsync();
            return View(customer);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer=await dbContext.Customer.FindAsync(id);
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Customer viewModel)
        { 
            var customer=await dbContext.Customer.FindAsync(viewModel.CustomerId);
            if(customer is not null)
            {
                customer.CustomerName = viewModel.CustomerName; 
                customer.CustomerId = viewModel.CustomerId;
                customer.Email = viewModel.Email;
                customer.Password = viewModel.Password;
                customer.Address = viewModel.Address;
                 
                await dbContext.SaveChangesAsync();
            
            }
            return RedirectToAction("List", "Customer");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Customer viewModel)
        {
            var customer = await dbContext.Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(x=> x.CustomerId==viewModel.CustomerId);
            if(customer is not null)
            {
                dbContext.Customer.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Customer");
        }
    }
}
