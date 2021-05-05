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

        public string FileHash { get; set; }

        public string Signature { get; set; }

        public string SymmetricKey { get; set; }

        public string SymmetricIV { get; set; }

        public MemberViewModel Member { get; set; }
    }
}
