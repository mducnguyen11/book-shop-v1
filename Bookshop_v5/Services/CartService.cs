using Bookshop_v5.Models.Domain;

namespace Bookshop_v5.Services
{
    public class CartService
    {
        private readonly DatabaseContext _context;

        public CartService(DatabaseContext context)
        {
            _context = context;
        }

        public Cart GetCart()
        {
            var cart = new Cart();

            // Lấy danh sách sản phẩm trong giỏ hàng
            var cartItems = _context.CartItem.ToList();

            // Thêm các sản phẩm vào giỏ hàng
            foreach (var item in cartItems)
            {
                var book = _context.Book.FirstOrDefault(b => b.Id == item.BookId);
                if (book != null)
                {
                    cart.AddItem(book, item.Quantity);
                }
            }

            return cart;
        }

        public void AddToCart(int bookId, int quantity)
        {
            var cartItem = _context.CartItem.FirstOrDefault(ci => ci.BookId == bookId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    BookId = bookId,
                    Quantity = quantity
                };
                _context.CartItem.Add(cartItem);
            }

            _context.SaveChanges();
        }

        public void RemoveFromCart(int bookId)
        {
            var cartItem = _context.CartItem.FirstOrDefault(ci => ci.BookId == bookId);

            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
                _context.SaveChanges();
            }
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItem.ToList();
            _context.CartItem.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }
}
