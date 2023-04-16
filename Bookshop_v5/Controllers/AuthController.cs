using Bookshop_v5.Interfaces;
using Bookshop_v5.Models.Domain;
using Bookshop_v5.Models.DTO;
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
                if (result.StatusCode == 2)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    TempData["msg"] = "Could not logged in..";
                    return RedirectToAction(nameof(Login));
                }

            }
        }

        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel registrationModel)
        {
            registrationModel.Role = "User";
            // if you want to register with user , Change Role="User"
            var result = await authService.RegisterAsync(registrationModel, _context);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("RegisterSuccess", "Auth");
            }
            else
            {
                TempData["msg"] = "Could not register .. !!!pls check again";
                return RedirectToAction(nameof(Register));
            }
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
