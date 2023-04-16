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
            var user = await _userManager.GetUserAsync(User);

            // lấy thông tin giỏ hàng của User, bao gồm danh sách các sản phẩm trong giỏ hàng
            var cartItems = _context.CartItem
                .Where(ci => ci.Cart.Id == user.CartId) // chỉ lấy các CartItem của User đang đăng nhập
                .Include(ci => ci.Book) // kết nối đối tượng Book vào CartItem
                .ToList();

            // tính tổng giá tiền của giỏ hàng
            var totalPrice = cartItems.Sum(ci => ci.UnitPrice);

            // gửi thông tin giỏ hàng đến View bằng biến ViewBag
            ViewBag.CartItems = cartItems;
            ViewBag.TotalPrice = totalPrice;

            return View();
        }

        public async Task<IActionResult> ConvertCartToOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartItems = _context.CartItem
                .Where(ci => ci.Cart.Id == user.CartId)
                .Include(ci => ci.Book)
                .ToList();

            if (cartItems.Count == 0)
            {
                // Nếu giỏ hàng trống thì chuyển hướng đến trang giỏ hàng
                return RedirectToAction("Index");
            }

            var orderItems = new List<OrderItem>();

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Book = cartItem.Book,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice
                };

                orderItems.Add(orderItem);
            }

            // Tạo một đơn hàng mới từ giỏ hàng
            var order = new Order
            {
                User = user,
                Items = orderItems,
                TotalPrice = cartItems.Sum(ci => ci.UnitPrice),
                OrderStatus = "Pending",
                DateCreated= DateTime.Now,                
            };

            // Lưu đơn hàng mới vào cơ sở dữ liệu
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // Xóa tất cả các CartItem trong giỏ hàng
            _context.CartItem.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // Cập nhật lại tổng giá tiền của giỏ hàng
            var cart = await _context.Cart.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == user.CartId);
            cart.TotalPrice = 0;
            foreach (var item in cart.Items)
            {
                cart.TotalPrice += item.UnitPrice;
            }
            await _context.SaveChangesAsync();

            // Chuyển hướng đến trang đơn hàng
            return RedirectToAction("Index", "Order", new { id = order.Id });
        }



    }
}
