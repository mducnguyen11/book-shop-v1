using Bookshop_v5.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Bookshop_v5.Models.DTO;

namespace Bookshop_v5.Controllers
{
    public class BookController : Controller
    {
        private readonly DatabaseContext _context;

        public BookController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index(int genreId, int page = 1, int pageSize = 2)
        {
		
			var genres = _context.Genre.ToList();
            IQueryable<Book> books = _context.Book.Include(b => b.Genre).Include(a => a.Author);

            // Lọc theo thể loại nếu được chỉ định
            if (genreId > 0)
            {
                books = books.Where(b => b.Genre.Id == genreId);
            }

            // Tính số lượng mục trong danh sách
            int totalItems = books.Count();

            // Tính số trang
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // Đảm bảo trang hiện tại không vượt quá số trang
            page = Math.Min(page, totalPages);

            // Đảm bảo trang hiện tại không nhỏ hơn 1
            page = Math.Max(page, 1);

            // Lấy các mục cho trang hiện tại
            IEnumerable<Book> items = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Tạo đối tượng PaginationViewModel để truyền dữ liệu đến View
            var model = new PaginationViewModel<Book>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                RouteValues = new Dictionary<string, int>
                {
                { "genre", genreId }
                },
                Genres = genres,
            };

            return View(model);
        }



        public IActionResult Details(int id)
        {
            var book = _context.Book.Include(b => b.Genre).Include(a => a.Author).FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
    }
}
