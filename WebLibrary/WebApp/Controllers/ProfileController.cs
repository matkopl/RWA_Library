using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BL.Models;
using BL.Security;
using BL.Services;
using BL.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace WebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ProfileController(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // GET: Profile/Index
        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _userRepository.Get(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var vm = _mapper.Map<UserDetailsVM>(user);
            return View(vm);
        }

        // POST: Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromBody] UserDetailsVM userDetailsVM)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(string.Join(", ", errors));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _userRepository.Get(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.FirstName = userDetailsVM.FirstName;
            user.LastName = userDetailsVM.LastName;
            user.Email = userDetailsVM.Email;
            user.Phone = userDetailsVM.Phone;

            _userRepository.Edit(userId, user);

            return Json(new { success = true, message = "Profile updated successfully." });
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
