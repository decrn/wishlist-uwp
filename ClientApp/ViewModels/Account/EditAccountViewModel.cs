using System.ComponentModel.DataAnnotations;

namespace ClientApp.ViewModels {
    public class EditAccountViewModel {

        [Required(ErrorMessage = "Please fill in your first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please fill in your last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please fill in your email address.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

    }
}
