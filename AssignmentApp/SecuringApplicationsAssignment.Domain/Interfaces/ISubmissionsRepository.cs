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

        void AddSubmission(Submission s);

    }
}
