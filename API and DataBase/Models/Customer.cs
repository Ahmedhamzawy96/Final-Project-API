using System.ComponentModel.DataAnnotations;

namespace API_and_DataBase.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<ExportReciept> ExportReciepts { get; set; } = new HashSet<ExportReciept>();

    }
}
