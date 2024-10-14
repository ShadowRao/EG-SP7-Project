using Connect_Collect.Data;
using Connect_Collect.Models.Entities;
using Connect_Collect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;


using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;
using System.Net.Http.Json;


namespace Connect_CollectTest
{
    [TestFixture]
    public class HomeControllerSignInAPItests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public HomeControllerSignInAPItests()
        {
            // Create the WebApplicationFactory
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Use in-memory database for testing
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseInMemoryDatabase("TestDb"));
                    });
                });

            // Create the HttpClient
            _client = _factory.CreateClient();
        }

        [SetUp]
        public async Task Setup()
        {
            // Insert test data for customers, sellers, admins
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Customer.RemoveRange(dbContext.Customer);
            dbContext.Seller.RemoveRange(dbContext.Seller);
            await dbContext.SaveChangesAsync();

            dbContext.Customer.Add(new Customer
            {
                //CustomerId = Guid.Parse("26496CEB-1CD4-4D27-6355-08DCEBB23E4E"),
                CustomerName="Customer",
                
                Email = "testcustomera@example.com",
                Password = new PasswordHasher<Customer>().HashPassword(null, "customerpassword")
            });

            dbContext.Seller.Add(new Seller
            {
                //SellerId = Guid.Parse("6F814961-8FF9-4EF2-8928-08DCEA7DECDA"),
                SellerName="Seller",
                Email = "testsellera@example.com",
                Contact="86123",
                Password = new PasswordHasher<Seller>().HashPassword(null, "sellerpassword")
            });

            dbContext.Admin.Add(new Admin
            {
                //AdminId = Guid.Parse("0658cedb-4118-400b-9a47-0653c9f9cfaa"),
                Email = "testadmin@example.com",
                Password = "adminpassword"
            });

            await dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task SignIn_Customer_ReturnsSuccess()
        {
            // Act: Perform SignIn with Customer credentials
            var signInModel = new SignInModel
            {
                Email = "testcustomer@example.com",
                Password = "customerpassword"
            };

            var response = await _client.PostAsJsonAsync("/Home/SignIn", signInModel);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Customer sign-in failed");
        }

        [Test]
        public async Task SignIn_Seller_ReturnsSuccess()
        {
            // Act: Perform SignIn with Seller credentials
            var signInModel = new SignInModel
            {
                Email = "testseller@example.com",
                Password = "sellerpassword"
            };

            var response = await _client.PostAsJsonAsync("/Home/SignIn", signInModel);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Seller sign-in failed");
        }

        [Test]
        public async Task SignIn_Admin_ReturnsSuccess()
        {
            // Act: Perform SignIn with Admin credentials
            var signInModel = new SignInModel
            {
                Email = "testadmin@example.com",
                Password = "adminpassword"
            };

            var response = await _client.PostAsJsonAsync("/Home/SignIn", signInModel);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Admin sign-in failed");
        }

        [Test]
        public async Task SignIn_InvalidCredentials_ReturnsError()
        {
            // Act: Perform SignIn with invalid credentials
            var signInModel = new SignInModel
            {
                Email = "invalid@example.com",
                Password = "invalidpassword"
            };

            var response = await _client.PostAsJsonAsync("/Home/SignIn", signInModel);

            // Assert
            Assert.IsFalse(response.IsSuccessStatusCode, "Invalid credentials should return error");
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            // Dispose the HttpClient or any resources if needed
            _client.Dispose();
            _factory.Dispose();
        }

    }
}
