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

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginVM { ReturnUrl = returnUrl };
            return View(model);
        }

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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");  
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var existingUser = _userRepository.GetAll().Any(u => u.UserName == registerVM.Username);
            if (existingUser)
            {
                ModelState.AddModelError("Username", "Couldn't register, username already taken");
                return View(registerVM);
            }

            var existingEmail = _userRepository.GetAll().Any(u => u.Email == registerVM.Email);
            if (existingEmail)
            {
                ModelState.AddModelError("Email", "Couldn't register, email already taken");
                return View(registerVM);
            }

            var salt = PasswordHashProvider.GetSalt();
            var hash = PasswordHashProvider.GetHash(registerVM.Password, salt);

            var user = _mapper.Map<User>(registerVM);
            user.PwdSalt = salt;
            user.PwdHash = hash;
            user.IsAdmin = false;

            _userRepository.Create(user);

            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
