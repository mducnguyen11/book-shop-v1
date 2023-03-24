using Bookshop_v5.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop_v5.Controllers
{
    [Authorize] // yêu cầu đăng nhập để truy cập vào các hành động trong CartController
    public class CartController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(DatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // lấy thông tin User đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            // lấy thông tin giỏ hàng của User, bao gồm danh sách các sản phẩm trong giỏ hàng
            var cart = await _context.Cart.Include(c => c.Items).ThenInclude(ci => ci.Book).FirstOrDefaultAsync(c => c.Id == user.CartId);


            // gửi thông tin giỏ hàng đến View bằng biến ViewBag
            ViewBag.Cart = cart;

            return View();
        }
    }
}
