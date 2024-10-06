using Microsoft.AspNetCore.Mvc;

namespace Connect_Collect.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
