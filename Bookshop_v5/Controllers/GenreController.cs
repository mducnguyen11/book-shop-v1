using Bookshop_v5.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_v5.Controllers
{
    public class GenreController : Controller
    {
        private readonly DatabaseContext _context;

        public GenreController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var genres = _context.Genre.ToList();
            return View(genres);
        }
    }
}
