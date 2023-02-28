using Bookshop_v5.Interfaces;
using Bookshop_v5.Models.DTO;
using Bookshop_v5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop_v5.Controllers
{
    public class AuthController : Controller
    {
        private IUserAuthServices authService;
       public AuthController(IUserAuthServices authService)
        {
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
    }
}
