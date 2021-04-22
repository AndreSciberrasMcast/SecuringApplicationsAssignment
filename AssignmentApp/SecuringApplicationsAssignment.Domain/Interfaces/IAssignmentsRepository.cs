using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Interfaces
{
    public interface IAssignmentsRepository
    {
        IQueryable<Assignment> GetAssignments();

        Assignment GetAssignment(Guid id);

        void DeleteAssignment(Assignment id);

        Guid AddAssignment(Assignment a);
    }
}
