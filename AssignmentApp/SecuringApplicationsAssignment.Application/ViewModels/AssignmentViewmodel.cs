using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuringApplicationsAssignment.Application.ViewModels
{
    public class AssignmentViewmodel
    {
       
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a deadline")]
        public DateTime Deadline { get; set; }

        public MemberViewModel Member{ get; set; }
    }
}
