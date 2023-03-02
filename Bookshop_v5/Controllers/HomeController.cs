using Bookshop_v5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bookshop_v5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
    }
}