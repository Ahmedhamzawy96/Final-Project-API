namespace API_and_DataBase.DTO
{
    public class ImportProductDTO
    {
        public int ImportReceiptID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal BuyingPrice { get; set; }
        public bool ISDeleted { get; set; }


    }
}

