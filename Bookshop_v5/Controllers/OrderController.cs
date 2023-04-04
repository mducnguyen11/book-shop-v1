using Bookshop_v5.Models.Domain;
using Bookshop_v5.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_v5.Controllers
{
    public class OrderController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(DatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
            var user = await _userManager.GetUserAsync(User);
            string userID = user.Id;

			var orders = _context.Order.Where(o => o.UserId == userID).ToList();

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

		public IActionResult Details(int id)
		{
			var order = _context.Order.Include(o => o.Items).FirstOrDefault(o => o.Id == id);

			if (order == null)
			{
				return NotFound();
			}

			return View(order);
		}

	}
}
