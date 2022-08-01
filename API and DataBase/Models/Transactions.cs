using System.ComponentModel.DataAnnotations;

namespace API_and_DataBase.Models
{
    public class Transactions
    {
        [Key]
        public int ID { get; set; }

        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string User { get; set; }
        public string Receiver { get; set; }

        public DateTime Date { get; set; }
        public int ReceiptID { get; set; }
        public string ReceiptType { get; set; }
    }
}
