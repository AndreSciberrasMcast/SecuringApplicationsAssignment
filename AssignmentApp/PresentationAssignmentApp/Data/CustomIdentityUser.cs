using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationAssignmentApp.Data
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public bool isTeacher { get; set; }

        public ApplicationUser assignedTeacher { get; set; }
    }
}
