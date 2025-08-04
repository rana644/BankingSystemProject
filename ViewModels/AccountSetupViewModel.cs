using BankingSystem1.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BankingSystem1.ViewModels
{
    public class AccountSetupViewModel
    {
        public int SelectedAccountTypeId { get; set; }
        public int SelectedBranchId { get; set; }
        public int SelectedCurrencyId { get; set; }


        [ValidateNever]
        public List<AccountType> AccountTypes { get; set; } // a dropdown list of account types current or saving
        [ValidateNever]
        public List<Branch> Branches { get; set; } // a dropdown list of branches to choose from same as those in the database 

       [ValidateNever]
        public List <Currency> Currencies { get; set; } // a dropdown list of currencies to choose from same as those in the database 
    }
}
