using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.ViewModels {
    public class ChangePasswordViewModel {

        [Required]
        [DataType(DataType.Password)]
        [MinLength(10)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(10)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
