using System.Collections.Generic;
using BankingSystem1.Models;

namespace BankingSystem1.ViewModels
{
    public class TransactionListViewModel
    {
        public List<Transaction> SentTransactions { get; set; } = new();
        public List<Transaction> ReceivedTransactions { get; set; } = new();
    }
}
