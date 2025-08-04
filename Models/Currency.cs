namespace BankingSystem1.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal ExchangeRateToEGP { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }

}
