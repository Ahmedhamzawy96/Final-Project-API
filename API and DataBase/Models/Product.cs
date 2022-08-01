using System.ComponentModel.DataAnnotations;

namespace API_and_DataBase.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<CarProduct> CarProducts { get; set; } = new HashSet<CarProduct>();
        public virtual ICollection<ExportProduct> ExportProducts { get; set; } = new HashSet<ExportProduct>();
        public virtual ICollection<ImportProduct> ImportProducts { get; set; } = new HashSet<ImportProduct>();


    }
}
