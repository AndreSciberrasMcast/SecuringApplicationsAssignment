using SecuringApplicationsAssignment.Data.Context;
using SecuringApplicationsAssignment.Domain.Interfaces;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApplicationsAssignment.Data.Repositories
{
    public class AssignmentsRepository : IAssignmentsRepository
    {
        MyDatabaseContext _context;

        public AssignmentsRepository(MyDatabaseContext context)
        {
            _context = context;
        }

        public Guid AddAssignment(Assignment a)
        {
            _context.Assignments.Add(a);
            _context.SaveChanges();
            return a.Id;
        }

        public void DeleteAssignment(Assignment a)
        {
            _context.Assignments.Remove(a);
            _context.SaveChanges();
        }

        public Assignment GetAssignment(Guid id)
        {
            return _context.Assignments.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Assignment> GetAssignments()
        {
            return _context.Assignments;
        }
    }
}
