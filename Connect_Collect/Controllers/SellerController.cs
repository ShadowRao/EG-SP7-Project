using Connect_Collect.Data;
using Connect_Collect.Models.Entities;
using Connect_Collect.Models;
using Microsoft.AspNetCore.Mvc;

namespace Connect_Collect.Controllers
{
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SellerController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Home(Guid Id)
        {
            var sellerdata = await dbContext.Seller.FindAsync(Id);
            return View(sellerdata);
        }
        [HttpGet]
        public IActionResult AddSeller()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSeller(AddSellerViewModel viewModel)
        {
            var seller = new Seller
            {
                SellerName = viewModel.SellerName,
                SellerId = viewModel.SellerId,
                Email = viewModel.Email,
                Password = viewModel.Password,
                Contact = viewModel.Contact,
                //Finish=viewModel.Finish
            };
            await dbContext.Seller.AddAsync(seller);
            await dbContext.SaveChangesAsync();
            return View();
        }

        
    }
}
