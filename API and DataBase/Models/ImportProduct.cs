using System.ComponentModel.DataAnnotations.Schema;

namespace API_and_DataBase.Models
{
    public class ImportProduct
    {
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        public bool ISDeleted { get; set; } = false;


        public virtual ImportReciept ImportReciept { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("ImportReciept")]
        public int ReceiptID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
    }
}
