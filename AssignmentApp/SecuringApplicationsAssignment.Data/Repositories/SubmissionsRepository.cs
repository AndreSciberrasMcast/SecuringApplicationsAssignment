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
            return _context.Submissions.SingleOrDefault(x => x.Assignment.Id == assignmentId && x.Member.Email == studentEmail);
        }

        public Submission GetSubmission(Guid submissionId)
        {
            return _context.Submissions.SingleOrDefault(x => x.Id == submissionId);
        }

        public void AddSubmission(Submission s)
        {
            _context.Submissions.Add(s);
            _context.SaveChanges();
        }

        public IQueryable<Submission> GetSubmissionsForASsignment(Guid assignmentID)
        {
            return _context.Submissions.Where(x => x.AssignmentId == assignmentID);
        }

        public IQueryable<Submission> GetSubmissionsByStudent(string email)
        {
            return _context.Submissions.Where(x => x.Member.Email == email);
        }

        public IQueryable<Comment> GetComments(Guid Id)
        {
            return _context.Comments.Where(x => x.SubmissionFk == Id).OrderBy(t => t.Time);
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}
