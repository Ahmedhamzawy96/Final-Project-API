using System.ComponentModel.DataAnnotations;

namespace API_and_DataBase.Models
{
    public class Expenses
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool ISDeleted { get; set; } = false;

    }
}
