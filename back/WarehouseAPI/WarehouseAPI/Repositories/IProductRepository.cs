using WarehouseAPI.Models;

namespace WarehouseAPI.Repositories
{
    public interface IProductRepository
    {
        Product? Search(string searchType, string searchValue);
    }
}