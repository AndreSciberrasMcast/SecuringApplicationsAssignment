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

        void DeleteAssignment(Guid id);
    }
}
