using System.ComponentModel.DataAnnotations;

namespace BankingSystem1.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
