using System.ComponentModel.DataAnnotations;

namespace healthspanmd.web.Models.Account
{
    public class AccountSettingsViewModel
    {
        
        public string UserRole { get; set; }

        public BasicInfo UserBasicInfo { get; set; }

        public class BasicInfo
        {
            public long UserId { get; set; }


            [MaxLength(50)]
            [Required]
            public string FirstName { get; set; }


            [MaxLength(50)]
            [Required]
            public string LastName { get; set; }


            [MaxLength(150)]
            public string Email { get; set; }


            [Required]
            [MaxLength(50)]
            [MinLength(10, ErrorMessage = "Please enter a valid phone")]
            public string Phone { get; set; }
        }
    }
}
