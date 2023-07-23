namespace API_and_DataBase.DTO
{
    public class TransactionsDTO
    {
        public int ID { get; set; }
        public int? AccountID { get; set; }
        public int AccountType { get; set; }
        public decimal? Paid { get; set; }
        public decimal? Remaining { get; set; }
        public int Type { get; set; }
        public int? OperationID { get; set; }
        public int Operation { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
        public string Name { get; set; }


    }
}