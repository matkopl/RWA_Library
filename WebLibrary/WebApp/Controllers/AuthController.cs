using AutoMapper;
using BL.Models;
using BL.Security;
using BL.Services;
using BL.Viewmodels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthController(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginVM { ReturnUrl = returnUrl };
            return View(model);
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);  
            }

            var user = _userRepository.GetAll().FirstOrDefault(u => u.UserName == loginVM.Username);

            if (user == null || PasswordHashProvider.GetHash(loginVM.Password, user.PwdSalt) != user.PwdHash)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(loginVM);  
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (loginVM.ReturnUrl != null)
            {
                return Redirect(loginVM.ReturnUrl);  
            }

            return RedirectToAction("Index", "Home");  
        }

        // POST: Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");  
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
