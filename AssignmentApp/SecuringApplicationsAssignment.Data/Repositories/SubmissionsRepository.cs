using SecuringApplicationsAssignment.Data.Context;
using SecuringApplicationsAssignment.Domain.Interfaces;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApplicationsAssignment.Data.Repositories
{
    public class SubmissionsRepository : ISubmissionsRepository
    {
        MyDatabaseContext _context;

        public SubmissionsRepository(MyDatabaseContext context)
        {
            _context = context;
        }

        public Submission GetSubmission(Guid assignmentId, string studentEmail)
        {
            return _context.Submissions.SingleOrDefault(x => x.Id == assignmentId && x.Member.Email == studentEmail);
        }

        public IQueryable<Submission> GetSubmissions(Guid assignmentID)
        {
            throw new NotImplementedException();
        }

        public void AddSubmission(Submission s)
        {
            _context.Submissions.Add(s);
            _context.SaveChanges();
        }
    }
}
