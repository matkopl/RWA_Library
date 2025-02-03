using AutoMapper;
using Azure.Identity;
using BL.Models;
using BL.Security;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BL.DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IRepository<User> userRepository, ILogRepository logRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _logRepository = logRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (_userRepository.GetAll().Any(u => u.UserName == registerDto.Username))
            {
                _logRepository.AddLog($"Couldn't register, username {registerDto.Username} already taken", 2);
                return BadRequest("Username already taken");
            }

            if (_userRepository.GetAll().Any(u => u.Email == registerDto.Email))
            {
                _logRepository.AddLog($"Couldn't register, email already taken", 2);
                return BadRequest("Email already taken");
            }

            string salt = PasswordHashProvider.GetSalt();
            string hash = PasswordHashProvider.GetHash(registerDto.Password, salt);

            var user = _mapper.Map<User>(registerDto);

            user.PwdSalt = salt;
            user.PwdHash = hash;
            user.IsAdmin = false;
      
            _userRepository.Create(user);

            _logRepository.AddLog($"Successfully registered user: {user.UserName} with id={user.Id}", 1);

            return Ok(new
            {
                registerDto.Username,
                registerDto.Email,
                registerDto.FirstName,
                registerDto.LastName,
                registerDto.Phone
            });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                var existingUser = _userRepository.GetAll().FirstOrDefault(u => u.UserName == loginDto.Username);
                if (existingUser == null)
                {
                    _logRepository.AddLog("Failed login", 2);
                    return BadRequest(genericLoginFail);
                }

                var b64hash = PasswordHashProvider.GetHash(loginDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                {
                    _logRepository.AddLog("Failed login", 2);
                    return BadRequest(genericLoginFail);
                }

                var role = existingUser.IsAdmin ? "Admin" : "User";

                var secureKey = _configuration["JWT:SecureKey"];
                var token = JwtTokenProvider.CreateToken(secureKey, 120, loginDto.Username, existingUser.IsAdmin);

                _logRepository.AddLog($"Successfully logged in with username: {loginDto.Username}", 1);
                return Ok(new { Token = token });
            }
            catch (Exception e)
            {
                _logRepository.AddLog("An error occurred while logging in", 3);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var existingUser = _userRepository.GetAll().FirstOrDefault(u => u.UserName == changePasswordDto.Username);
                if (existingUser == null)
                {
                    _logRepository.AddLog("Password change failed, user not found", 2);
                    return NotFound();
                }

                var oldHash = PasswordHashProvider.GetHash(changePasswordDto.CurrentPassword, existingUser.PwdSalt);
                if (oldHash != existingUser.PwdHash)
                {
                    _logRepository.AddLog($"Password change failed for user: {changePasswordDto.Username}, current password incorrect", 2);
                    return BadRequest();
                }

                var newSalt = PasswordHashProvider.GetSalt();
                var newHash = PasswordHashProvider.GetHash(changePasswordDto.NewPassword, newSalt);

                existingUser.PwdSalt = newSalt;
                existingUser.PwdHash = newHash;

                _userRepository.Edit(existingUser.Id, existingUser);

                _logRepository.AddLog($"Successfully changed password for user {changePasswordDto.Username}", 1);
                return Ok("Password has been successfully changed");
            }
            catch (Exception e)
            {
                _logRepository.AddLog("An error occurred while changing password", 3);
                return StatusCode(500, e.Message);
            }
        }
    }
}
