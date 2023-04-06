using Bookshop_v5.Models.Domain;
using Bookshop_v5.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Bookshop_v5.Controllers
{
    public class AdminController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Book(int genreId, int page = 1, int pageSize = 2)
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

        [Authorize(Roles = "Admin")]
        public IActionResult CreateBook()
        {
            var genres = _context.Genre.ToList();
            var authors = _context.Author.ToList();

            var model = new BookCreateViewModel
            {
                Genres = genres,
                Authors = authors
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(BookCreateViewModel model)      {


            try
            {
                var book = new Book
                {
                    Name = model.Name,
                    Description = model.Description,
                    GenreId = model.GenreId,
                    AuthorId = model.AuthorId,
                    OldPrice = model.OldPrice,
                    Price = model.Price,
                    Image = model.Image // Lưu trữ link ảnh vào cơ sở dữ liệu
                };

                _context.Book.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                model.Genres = _context.Genre.ToList();
                model.Authors = _context.Author.ToList();

                return View(model);
            }


            
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _context.Book.Include(b => b.Genre).Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var genres = _context.Genre.ToList();
            var authors = _context.Author.ToList();

            var model = new BookEditViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description,
                GenreId = book.GenreId,
                AuthorId = book.AuthorId,
                OldPrice = book.OldPrice,
                Price = book.Price,
                Image = book.Image,
                Genres = genres,
                Authors = authors
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]       
        public async Task<IActionResult> EditBook(int id, BookEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            
                try
                {
                    var book = await _context.Book.FindAsync(id);

                    book.Name = model.Name;
                    book.Description = model.Description;
                    book.GenreId = model.GenreId;
                    book.AuthorId = model.AuthorId;
                    book.OldPrice = model.OldPrice;
                    book.Price = model.Price;
                    book.Image = model.Image;

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Book));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }           

           
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(b => b.Id == id);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            try
            {
                _context.Book.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Book));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the book.");
                return View("EditBook", new BookEditViewModel { Id = book.Id });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Order(int page = 1, int pageSize = 10)
        {
            var orders = _context.Order.ToList();

            int totalItems = orders.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginationViewModel = new PaginationViewModel<Order>
            {
                Items = orders.Skip((page - 1) * pageSize).Take(pageSize),
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                RouteValues = new Dictionary<string, int> { { "page", page }, { "pageSize", pageSize } },
            };

            return View(paginationViewModel);
        }


        public IActionResult AcceptOrder(int id)
        {
            var order = _context.Order.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Accepted";
            _context.SaveChanges();

            return RedirectToAction("Order", "Admin");
        }

        public IActionResult RejectOrder(int id)
        {
            var order = _context.Order.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Rejected";
            _context.SaveChanges();

            return RedirectToAction("Order", "Admin");
        }

    }
}
