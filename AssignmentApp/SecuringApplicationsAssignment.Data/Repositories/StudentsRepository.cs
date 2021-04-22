using SecuringApplicationsAssignment.Data.Context;
using SecuringApplicationsAssignment.Domain.Interfaces;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Data.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {

        MyDatabaseContext _context;

        public StudentsRepository(MyDatabaseContext context)
        {
            _context = context;
        }

        public Guid AddStudent(Student s)
        {
            _context.Students.Add(s);
            _context.SaveChanges();
            return s.Id;
        }
    }
}
