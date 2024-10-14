using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Connect_CollectTest
{
    [TestFixture]
    public class HomeControllerAPItests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public HomeControllerAPItests()
        {
            // Create the WebApplicationFactory
            _factory = new WebApplicationFactory<Program>();
            // Create the HttpClient
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task SignIn_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/Home/SignIn"); // Adjust the endpoint as needed

            // Assert
            response.EnsureSuccessStatusCode(); // This will throw if the status code is not a success code
        }

        [Test]
        public async Task Home_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/Home/Index"); // Adjust the endpoint as needed

            // Assert
            response.EnsureSuccessStatusCode(); // This will throw if the status code is not a success code
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
