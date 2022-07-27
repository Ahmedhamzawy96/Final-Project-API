﻿namespace API_and_DataBase.Models
{
    public class ExportProduct
    {
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }


        public virtual ExportReciept ExportReciept { get; set; }
        public virtual Product Product { get; set; }

        public int ReceiptID { get; set; }
        public int ProductID { get; set; }
    }
}
