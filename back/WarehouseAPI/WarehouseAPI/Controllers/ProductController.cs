using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Models;
using WarehouseAPI.Repositories;
using System.Text;

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string searchType, [FromQuery] string searchValue)
        {
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

                if (username != "admin" || password != "12345")
                {
                    return StatusCode(403, new { message = "Только Админ может добавлять товары!" });
                }

                _repository.Add(product);
                return Ok(new { message = "Товар успешно добавлен!" });
            }
            catch
            {
                return Unauthorized(new { message = "Ошибка авторизации" });
            }
        }
    }
}