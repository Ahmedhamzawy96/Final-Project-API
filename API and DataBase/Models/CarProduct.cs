namespace API_and_DataBase.Models
{
    public class CarProduct
    {
        public int Quantity { get; set; }

        public virtual Car Car { get; set; }
        public virtual Product Product { get; set; }

        public int CarID { get; set; }
        public int ProductID { get; set; }
    }
}
