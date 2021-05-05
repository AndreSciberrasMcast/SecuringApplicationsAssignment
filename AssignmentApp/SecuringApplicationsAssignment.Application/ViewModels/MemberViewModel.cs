using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuringApplicationsAssignment.Application.ViewModels
{
    public class MemberViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Name is not valid. Use only letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Surname is not valid. Use only letters")]
        public string LastName { get; set; }

        public string TeacherEmail { get; set; }

        public string PrivateKey { get; set; }

        public string PublicKey { get; set; }

        
    }
}
