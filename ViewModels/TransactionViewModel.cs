using BankingSystem1.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem1.ViewModels
{
    public class TransactionViewModel
    {
        public int SenderAccountId { get; set; }
        public string ReceiverPhoneNo { get; set; }
        public int ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Value Date")]
        public DateTime? ValueDate { get; set; }

        [Display(Name = "Need Job")]
        public bool NeedJob { get; set; }


        public List<Account> SenderAccounts { get; set; } = new();
        public List<Account> ReceiverAccounts { get; set; } = new();
    }
}
