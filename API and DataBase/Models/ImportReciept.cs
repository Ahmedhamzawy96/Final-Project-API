using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_and_DataBase.Models
{
    public class ImportReciept
    {
        [Key]
        public int ID { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public decimal Paid { get; set; }
        public decimal Remaining { get; set; }


        public virtual Supplier Supplier { get; set; }
        public virtual Users User { get; set; }


        [ForeignKey("Supplier")]
        public int? SupplierID { get; set; }
        [ForeignKey("User")]
        public string UserName { get; set; }

        public virtual ICollection<ImportProduct> ImportProducts { get; set; } = new HashSet<ImportProduct>();


    }
}
