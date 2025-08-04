namespace BankingSystem1.Models
{
    public class ParsedTransactionData
    {
        public int TransactionId { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderAccountType { get; set; }
        public string SenderCurrency { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverAccountType { get; set; }
        public string ReceiverCurrency { get; set; }
        public decimal SenderAmount { get; set; }
        public decimal ReceiverAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime TransactionTimestamp { get; set; }
    }
}
