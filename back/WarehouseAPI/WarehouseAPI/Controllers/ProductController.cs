using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Models;
using WarehouseAPI.Repositories;
using WarehouseAPI.Data;
using System.Text;

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductRepository repository, ApplicationDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string searchType, [FromQuery] string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchType))
            {
                return BadRequest(new { message = "Тип поиска не может быть пустым." });
            }

            if (searchValue == null)
            {
                return BadRequest(new { message = "Значение для поиска не передано." });
            }

            var product = _repository.Search(searchType, searchValue);

            if (product == null) return Ok(null);

            return Ok(product);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] Product product)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Unauthorized(new { message = "Нужна авторизация!" });
            }

            string authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
            {
                return Unauthorized(new { message = "Неверный формат авторизации!" });
            }

            try
            {
                var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                var encoding = Encoding.GetEncoding("iso-8859-1");
                var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                var parts = usernamePassword.Split(':');
                var username = parts[0];
                var password = parts[1];

                var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

                if (user == null || user.Role != "admin")
                {
                    return StatusCode(403, new { message = "Доступ запрещен. Только Администратор может добавлять товары!" });
                }

                _repository.Add(product);
                return Ok(new { message = "Товар успешно добавлен!" });
            }
            catch
            {
                return Unauthorized(new { message = "Ошибка при чтении авторизации" });
            }
        }
    }
}