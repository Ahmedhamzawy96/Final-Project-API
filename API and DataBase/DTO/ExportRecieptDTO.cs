namespace API_and_DataBase.DTO
{
    public class ExportRecieptDTO
    {
        public int ID { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public decimal Paid { get; set; }
        public decimal Remaining { get; set; }

        public int? CustID { get; set; }
        public string? CustName { get; set; }

        public string UserName { get; set; }

        public int? CarSellID { get; set; }
        public int? CarBuyID { get; set; }

    }
}
