using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Application.Interfaces
{
    public interface IStudentsService
    {
        void AddStudent(Student s);
    }
}
