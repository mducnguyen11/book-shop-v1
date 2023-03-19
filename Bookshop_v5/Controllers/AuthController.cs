using Bookshop_v5.Interfaces;
using Bookshop_v5.Models.Domain;
using Bookshop_v5.Models.DTO;
using Bookshop_v5.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookshop_v5.Controllers
{
    public class AuthController : Controller
    {
        private IUserAuthServices authService;
        private readonly DatabaseContext _context;
        public AuthController(DatabaseContext context, IUserAuthServices authService)
        {
            _context = context;
            this.authService = authService;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await authService.LoginAsync(model);
            if (result.StatusCode == 1)
                return RedirectToAction("Index", "Home");
            else
            {
                TempData["msg"] = "Could not logged in..";
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Register()
        {
            var model = new RegistrationModel
            {
               Email = "ducadmin@gmail.com",
                Username = "admin",
                Name = "Duc NM",
                Password = "Admin@123",             
                Role = "Admin",
                Birthday= DateTime.Now,
                Address = "Ha noi",
                Gender ="Male"
            };
            // if you want to register with user , Change Role="User"
            var result = await authService.RegisterAsync(model);
            return Ok(result.Message);
        }


        public async Task<IActionResult> Profile()
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileModel
            {
                Name = user.Name,
                Gender = user.Gender,
                Address = user.Address,
                Birthday = user.Birthday,
                Email = user.Email,
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileModel
            {
                Name = user.Name,
                Gender = user.Gender,
                Address = user.Address,
                Birthday = user.Birthday,
                Email = user.Email,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.Gender = model.Gender;
            user.Address = model.Address;
            user.Birthday = model.Birthday;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}
