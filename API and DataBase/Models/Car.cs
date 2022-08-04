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

        public virtual ICollection<CarProduct> CarProducts { get; set; } = new HashSet<CarProduct>();//API GET products of car  (id car )

        [InverseProperty("CarBuy")]
        public virtual ICollection<ExportReciept> BuyReciepts { get; set; } = new HashSet<ExportReciept>();

        [InverseProperty("CarSell")]
        public virtual ICollection<ExportReciept> SellReciepts { get; set; } = new HashSet<ExportReciept>();
        public virtual ICollection<Users> Users { get; set; } = new HashSet<Users>();
    }
}
