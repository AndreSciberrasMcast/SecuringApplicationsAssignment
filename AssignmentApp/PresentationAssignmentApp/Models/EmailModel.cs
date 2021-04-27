using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationAssignmentApp.Models
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
