using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Application.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        public string Data { get; set; }

        public SubmissionViewModel Submission { get; set; }

        public MemberViewModel Member { get; set; }
    }
}
