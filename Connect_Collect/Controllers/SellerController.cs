﻿using Connect_Collect.Data;
using Connect_Collect.Models.Entities;
using Connect_Collect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [HttpGet]
        public IActionResult AddProduct()
        {
            // Populate the seller list for the dropdown
            ViewBag.SellerList = new SelectList(dbContext.Seller, "SellerId", "SellerName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (model.ImageFile != null)
            {
                // Get the path to save the image
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

                // Create a unique filename for the image
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName); // Keep the file extension
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image to the file system
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                // Save only the image URL to the database
                model.ImageUrl = "/images/" + uniqueFileName;
                var product = new Product
                {
                    ProductId = model.ProductId,
                    ProductName = model.ProductName,
                    SellerId = model.SellerId,
                    ProductDescription = model.ProductDescription,  
                    ImageUrl = model.ImageUrl,
                    Price = model.Price,

                };
                // Add the model to the database
                dbContext.Product.Add(product);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("ViewProducts");

            }

            return View(model);  // Handle case when no file is uploaded
        }




    }
}
