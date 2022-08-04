namespace API_and_DataBase.DTO
{
    public class ProductDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
    }
}
