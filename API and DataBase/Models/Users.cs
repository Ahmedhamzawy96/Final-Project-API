using System.ComponentModel.DataAnnotations;

namespace API_and_DataBase.Models
{
    public class Users
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Type { get; set; }

        public virtual ICollection<ImportReciept> ImportReciepts { get; set; } = new HashSet<ImportReciept>();
        public virtual ICollection<ExportReciept> ExportReciepts { get; set; } = new HashSet<ExportReciept>();

    }
}
