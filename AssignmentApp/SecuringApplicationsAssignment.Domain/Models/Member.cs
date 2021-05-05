using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Models
{
    public class Member
    {
        [Key]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string TeacherEmail { get; set; }

        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

      
    }
}
