using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_and_DataBase.Models
{
    public class Car
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Account { get; set; }
        public string Notes { get; set; }
        public bool ISDeleted { get; set; }
        public virtual ICollection<CarProduct> CarProducts { get; set; } = new HashSet<CarProduct>();//API GET products of car  (id car )

        public virtual ICollection<ExportReciept> ExportReciepts { get; set; } = new HashSet<ExportReciept>();

        public virtual ICollection<Users> Users { get; set; } = new HashSet<Users>();
    }
}
