using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Application.ViewModels
{
    public class SubmissionViewModel
    {

        public Guid Id { get; set; }

        public AssignmentViewmodel Assignment { get; set; }

        public string FilePath { get; set; }

        public MemberViewModel Member { get; set; }
    }
}
