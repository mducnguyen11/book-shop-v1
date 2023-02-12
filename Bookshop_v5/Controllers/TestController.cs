using Microsoft.AspNetCore.Mvc;

namespace Bookshop_v5.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
