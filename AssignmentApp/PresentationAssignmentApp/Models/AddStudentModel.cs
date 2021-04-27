using SecuringApplicationsAssignment.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationAssignmentApp.Models
{
    public class AddStudentModel
    {
        
        public EmailModel EmailModel { get; set; }

        public MemberViewModel MemberModel { get; set; }
    }
}
