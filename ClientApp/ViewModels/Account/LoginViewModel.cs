using System.ComponentModel.DataAnnotations;

namespace ClientApp.ViewModels {
    public class LoginViewModel {
        [Required(ErrorMessage = "Please fill in your email address.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please fill in a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
