using System.ComponentModel.DataAnnotations;

namespace ServerApp.ViewModels {
    public class RegisterViewModel {

        [Required(ErrorMessage = "Please fill in your first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please fill in your last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please fill in your email address.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please fill in a password.")]
        [MinLength(10, ErrorMessage = "Password is too short, minimum length is 10 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }
}
