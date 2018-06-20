using System.ComponentModel.DataAnnotations;

namespace ClientApp.ViewModels {
    public class ForgotPasswordViewModel {
        [Required(ErrorMessage = "Please fill in your email address.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }
    }
}
