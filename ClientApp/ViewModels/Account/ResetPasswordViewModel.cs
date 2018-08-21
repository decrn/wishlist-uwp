using System.ComponentModel.DataAnnotations;

namespace ClientApp.ViewModels {
    public class ResetPasswordViewModel {
        [Required(ErrorMessage = "Please fill in your email address.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please fill in a new password.")]
        [MinLength(10, ErrorMessage = "Password is too short, minimum length is 10 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please fill in the code you got in your mailbox.")]
        public string Code { get; set; }
    }
}
