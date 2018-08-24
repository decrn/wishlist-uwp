using System.ComponentModel.DataAnnotations;

namespace ClientApp.ViewModels {
    public class ChangePasswordViewModel {

        [Required(ErrorMessage = "Please fill in your old password.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please fill in a new password.")]
        [MinLength(10, ErrorMessage = "Password is too short, minimum length is 10 characters.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage="New passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }
}
