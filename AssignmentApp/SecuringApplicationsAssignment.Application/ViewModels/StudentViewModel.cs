using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuringApplicationsAssignment.Application.ViewModels
{
    public class StudentViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Name is not valid. Use only letters")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Surname is not valid. Use only letters")]
        public string LastName { get; set; }
    }
}
