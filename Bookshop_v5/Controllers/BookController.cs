using Bookshop_v5.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Bookshop_v5.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace Bookshop_v5.Controllers
{
    public class BookController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;

        public BookController(DatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int genreId, int page = 1, int pageSize = 7, string search = "")
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

        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId, int quantity)
        {
            // lấy thông tin User đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            // lấy thông tin giỏ hàng của User
            // lấy thông tin giỏ hàng của User, bao gồm danh sách các sản phẩm trong giỏ hàng
            var cart = await _context.Cart.Include(c => c.Items).ThenInclude(ci => ci.Book).FirstOrDefaultAsync(c => c.Id == user.CartId);

            // lấy thông tin sách muốn thêm vào giỏ hàng
            var book = await _context.Book.FindAsync(bookId);

            if (book == null)
            {
                return NotFound();
            }

            // kiểm tra xem Cart đã tồn tại CartItem có chứa sách này hay chưa
            var existingCartItem = cart.Items.FirstOrDefault(ci => ci.BookId == bookId);

            if (existingCartItem != null)
            {
                // nếu đã có CartItem chứa sách này thì cộng thêm quantity vào CartItem đó
                existingCartItem.Quantity += quantity;
                existingCartItem.UnitPrice = book.Price * existingCartItem.Quantity;
            }
            else
            {
                // nếu chưa có CartItem chứa sách này thì tạo mới một CartItem và thêm vào giỏ hàng
                var cartItem = new CartItem
                {
                    BookId = bookId,
                    Quantity = quantity,
                    CartId = user.CartId,
                    UnitPrice = book.Price * quantity,
                };

                cart.Items.Add(cartItem);
            }

            // cập nhật giỏ hàng trong CSDL
            cart.TotalPrice = cart.Items.Sum(ci => ci.UnitPrice);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
