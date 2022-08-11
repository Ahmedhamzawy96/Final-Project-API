namespace API_and_DataBase.DTO
{
    public class ExportRecieptDTO
    {
        public int ID { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public string Date { get; set; }
        public decimal Paid { get; set; }
        public decimal Remaining { get; set; }
        public int? customerID { get; set; }
        public string UserName { get; set; }
        public int? CarID { get; set; }
        public ExportProductDTO [] Products { get; set; }

    }
}
