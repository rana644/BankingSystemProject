using System.ComponentModel.DataAnnotations;

namespace BankingSystem1.ViewModels
{
    public class AccountCreationRequestViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
