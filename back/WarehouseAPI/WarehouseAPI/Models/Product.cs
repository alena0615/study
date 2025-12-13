namespace WarehouseAPI.Models
{
    public class Product
    {
        public string CellLocation { get; set; } = string.Empty;
        public DateTime ReceivedDate { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ReceivedByEmployee { get; set; } = string.Empty;
        public string? IssuedByEmployee { get; set; }
        public double WeightReceived { get; set; }
        public double? WeightIssued { get; set; }
        public string? Barcode { get; set; }
        public string? StorageCell { get; set; }
        public string? ProductBarcode { get; set; }
        public string? ArticleNumber { get; set; }
    }
}