using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Interfaces
{
    public interface ISubmissionsRepository
    {
        IQueryable<Submission> GetSubmissionsForASsignment(Guid assignmentID);

        IQueryable<Submission> GetSubmissionsByStudent(string email);

        Submission GetSubmission(Guid assignmentID, string userEmail);

        Submission GetSubmission(Guid submissionID);

        void AddSubmission(Submission s);

        IQueryable<Comment> GetComments(Guid Id);

        void AddComment(Comment comment);

    }
}
