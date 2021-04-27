using SecuringApplicationsAssignment.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApplicationsAssignment.Application.Interfaces
{
    public interface IAssignmentsService
    {
        IQueryable<AssignmentViewmodel> GetAssignments();

        AssignmentViewmodel GetAssignment(Guid id);

        void AddAssignment(AssignmentViewmodel assignment);

        void AddSubmission(SubmissionViewModel submission);

        SubmissionViewModel GetSubmission(Guid assignmentId, string studentEmail);

        SubmissionViewModel GetSubmission(Guid id);

        IQueryable<SubmissionViewModel> GetSubmissions(string email);

        IQueryable<SubmissionViewModel> GetSubmissions(Guid assignmentId);

        IQueryable<CommentViewModel> GetComments(Guid id);

        void AddComment(CommentViewModel comment);

        void DeleteAssignment(Guid id);
    }
}
