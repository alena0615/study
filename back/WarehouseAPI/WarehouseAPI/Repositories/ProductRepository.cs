using WarehouseAPI.Models;

namespace WarehouseAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new()
        {
            new Product
            {
                CellLocation = "A-15-03",
                ReceivedDate = DateTime.Parse("2024-12-01T10:30:00"),
                IssuedDate = DateTime.Parse("2024-12-05T14:20:00"),
                ProductName = "Ноутбук Lenovo ThinkPad X1 Carbon",
                ReceivedByEmployee = "Иванов Иван Иванович",
                IssuedByEmployee = "Петров Петр Петрович",
                WeightReceived = 1.2,
                WeightIssued = 1.2,
                Barcode = "1234567890123",
                StorageCell = "A-15-03",
                ProductBarcode = "LNV-X1C-2024",
                ArticleNumber = "ART-001"
            },
            new Product
            {
                CellLocation = "B-22-07",
                ReceivedDate = DateTime.Parse("2024-12-03T09:15:00"),
                IssuedDate = null,
                ProductName = "Монитор Samsung 27 4K",
                ReceivedByEmployee = "Сидорова Мария Александровна",
                IssuedByEmployee = null,
                WeightReceived = 5.8,
                WeightIssued = null,
                Barcode = "9876543210987",
                StorageCell = "B-22-07",
                ProductBarcode = "SMS-27-4K",
                ArticleNumber = "ART-002"
            },
            new Product
            {
                CellLocation = "C-10-12",
                ReceivedDate = DateTime.Parse("2024-11-28T11:45:00"),
                IssuedDate = DateTime.Parse("2024-12-02T16:30:00"),
                ProductName = "Клавиатура механическая Keychron K8",
                ReceivedByEmployee = "Козлов Алексей Дмитриевич",
                IssuedByEmployee = "Морозова Ольга Викторовна",
                WeightReceived = 0.9,
                WeightIssued = 0.9,
                Barcode = "5555666677778",
                StorageCell = "C-10-12",
                ProductBarcode = "KEY-K8-PRO",
                ArticleNumber = "ART-003"
            },
            new Product
            {
                CellLocation = "D-05-20",
                ReceivedDate = DateTime.Parse("2024-12-07T08:00:00"),
                IssuedDate = null,
                ProductName = "Мышь Logitech MX Master 3",
                ReceivedByEmployee = "Новиков Сергей Павлович",
                IssuedByEmployee = null,
                WeightReceived = 0.14,
                WeightIssued = null,
                Barcode = "1111222233334",
                StorageCell = "D-05-20",
                ProductBarcode = "LOG-MX3",
                ArticleNumber = "ART-004"
            }
        };

        public Product? Search(string searchType, string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue)) return null;

            var val = searchValue.ToLower().Trim();

            return _products.FirstOrDefault(p =>
            {
                return searchType switch
                {
                    "barcode" => p.Barcode?.ToLower() == val,
                    "storageCell" => p.StorageCell?.ToLower() == val,
                    "productBarcode" => p.ProductBarcode?.ToLower() == val,
                    "article" => p.ArticleNumber?.ToLower() == val,
                    _ => false
                };
            });
        }
    }
}