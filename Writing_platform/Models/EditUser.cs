using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Writing_platform.Models
{
    public class EditUser
    {
        [DisplayName("E-Mail")]
        public string Email { get; set; }

        [DisplayName("Current password")]
        [DataType(DataType.Password)]
        public string CurrentEmailPassword { get; set; }

        [DisplayName("Current password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}