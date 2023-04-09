using Bookshop_v5.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

        public IActionResult Details(int id)
        {
            var genre = _context.Genre.Include(g => g.Books).FirstOrDefault(g => g.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }


        // Action method hiển thị trang tạo mới thể loại sách
        public IActionResult Create()
        {
            var genre = new Genre();
            return View(genre);
        }

        // Action method POST để lưu thông tin thể loại sách vào cơ sở dữ liệu
        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Genre.Add(genre);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        // Action method hiển thị trang chỉnh sửa thông tin thể loại sách
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Debug.WriteLine("----------------------------------------------");
            if (id == 0)
         {
         return NotFound();
         }
         var genre = _context.Genre.Find(id);
         return View(genre);
         }


        // Action method POST để lưu thông tin thể loại sách được sửa đổi vào cơ sở dữ liệu
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name")] Genre genre)
        {          
            _context.Entry(genre).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var genre = _context.Genre.Find(id);
            _context.Genre.Remove(genre);
            _context.SaveChanges();
            return RedirectToAction("Index");


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var genre = _context.Genre.Find(id);
            _context.Genre.Remove(genre);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
