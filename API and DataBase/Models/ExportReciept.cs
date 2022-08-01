﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_and_DataBase.Models
{
    public class ExportReciept
    {
        [Key]
        public int ID { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public decimal Paid { get; set; }
        public decimal Remaining { get; set; }



        public virtual Customer Customer { get; set; }
        public virtual Users User { get; set; }

        [InverseProperty("SellReciepts")]
        public virtual Car CarSell { get; set; }

        [InverseProperty("BuyReciepts")]
        public virtual Car CarBuy { get; set; }



        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }
        [ForeignKey("User")]
        public string UserName { get; set; }
        [ForeignKey("Car")]
        public int? CarSellID { get; set; }
        [ForeignKey("Car")]
        public int? CarBuyID { get; set; }
        public virtual ICollection<ExportProduct> ExportProducts { get; set; } = new HashSet<ExportProduct>();
    }
}