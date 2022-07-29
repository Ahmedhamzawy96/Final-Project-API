using System.ComponentModel.DataAnnotations.Schema;

namespace API_and_DataBase.Models
{
    public class CarProduct
    {
        public int Quantity { get; set; }

        public virtual Car Car { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Car")]

        public int CarID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
    }
}
