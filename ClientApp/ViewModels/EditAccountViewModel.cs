using System.ComponentModel.DataAnnotations;

namespace ClientApp.ViewModels {
    public class EditAccountViewModel {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
