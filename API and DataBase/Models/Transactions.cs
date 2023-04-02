using API_and_DataBase.Structures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_and_DataBase.Models
{
    public class Transactions
    {
        [Key]
        public int ID { get; set; }
        public int? AccountID { get; set; }
        public int AccountType { get; set; }
        public decimal? Paid { get; set; }
        public decimal? Remaining { get; set; }
        public int Type { get; set; }
        public int? OperationID { get; set; }
        public int Operation { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("User")]
        public string UserName { get; set; }
        public virtual Users User { get; set; }
        public string Notes { get; set; }
        public bool ISDeleted { get; set; } = false;

    }
}
