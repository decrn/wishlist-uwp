using System.ComponentModel.DataAnnotations;

namespace ClientApp.ViewModels {
    public class ResetPasswordViewModel {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(10)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
