using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Data;
using WarehouseAPI.Models;
using System.Text.RegularExpressions; 

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == request.Password);
            if (user == null) return Unauthorized(new { message = "Неверный логин или пароль" });
            return Ok(new { role = user.Role });
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginRequest request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return BadRequest(new { message = "Такой пользователь уже существует!" });
            }

            var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");

            if (!regex.IsMatch(request.Password))
            {
                return BadRequest(new { message = "Пароль слишком простой! Нужны буквы, цифры и минимум 6 знаков." });
            }

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = request.Password, 
                Role = "employee"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "Регистрация успешна! Теперь войдите." });
        }
    }
}