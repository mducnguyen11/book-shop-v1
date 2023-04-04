using Bookshop_v5.Models;
using Bookshop_v5.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bookshop_v5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Book.OrderByDescending(b => b.SoldQuantity).Take(4).ToList();
            ViewBag.TopSeller = books;
			var sciFiBooks = _context.Book.Take(5).ToList();
            // Where(b => b.Genre.Name == "Science fiction")


            ViewBag.SciFi = sciFiBooks;

			return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact model)
        {
            

            return View(model);
        }

    }
}