using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Interfaces
{
    public interface IStudentsRepository
    {

        Guid AddStudent(Student s);

    }
}
