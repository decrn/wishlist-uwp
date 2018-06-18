using System.ComponentModel.DataAnnotations;

namespace ServerApp.ViewModels {
    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
