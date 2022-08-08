namespace API_and_DataBase.DTO
{
    public class ExportProductDTO
    {
        public int ExportReceiptID { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
