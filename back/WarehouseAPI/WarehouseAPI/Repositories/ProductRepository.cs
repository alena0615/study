using Microsoft.EntityFrameworkCore;
using Npgsql;
using WarehouseAPI.Data;
using WarehouseAPI.Models;

namespace WarehouseAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public ProductRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public Product? Search(string searchType, string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue)) return null;
            var val = searchValue.ToLower().Trim();
            var query = _context.Products.AsQueryable();

            switch (searchType)
            {
                case "barcode":
                    return query.FirstOrDefault(p => p.Barcode != null && p.Barcode.ToLower() == val);
                case "storageCell":
                    return query.FirstOrDefault(p => p.CellLocation.ToLower() == val || (p.StorageCell != null && p.StorageCell.ToLower() == val));
                case "productBarcode":
                    return query.FirstOrDefault(p => p.ProductBarcode != null && p.ProductBarcode.ToLower() == val);
                case "article":
                    return query.FirstOrDefault(p => p.ArticleNumber != null && p.ArticleNumber.ToLower() == val);
                default:
                    return null;
            }
        }

        public void Add(Product p)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var sql = @"
                INSERT INTO ""Products"" 
                (
                    ""ProductName"", 
                    ""CellLocation"", 
                    ""WeightReceived"", 
                    ""ReceivedDate"", 
                    ""ReceivedByEmployee"",
                    ""Barcode"", 
                    ""ArticleNumber"", 
                    ""ProductBarcode""
                ) 
                VALUES 
                (
                    @Name, 
                    @Cell, 
                    @Weight, 
                    @Date, 
                    @Emp, 
                    @Bar, 
                    @Art, 
                    @ProdBar
                )";

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", p.ProductName);
            command.Parameters.AddWithValue("@Cell", p.CellLocation);
            command.Parameters.AddWithValue("@Weight", p.WeightReceived);
            command.Parameters.AddWithValue("@Date", DateTime.Now);
            command.Parameters.AddWithValue("@Emp", "Администратор");

            command.Parameters.AddWithValue("@Bar", p.Barcode ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Art", p.ArticleNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ProdBar", p.ProductBarcode ?? (object)DBNull.Value);

            command.ExecuteNonQuery();
        }
    }
}