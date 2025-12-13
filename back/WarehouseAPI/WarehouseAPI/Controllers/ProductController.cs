using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Repositories;

namespace WarehouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string searchType, [FromQuery] string searchValue)
        {
            var product = _repository.Search(searchType, searchValue);

            if (product == null)
            {
                return Ok(null);
            }

            return Ok(product);
        }
    }
}