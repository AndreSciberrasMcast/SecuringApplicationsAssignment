using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;
using SecuringApplicationsAssignment.Data.Repositories;
using SecuringApplicationsAssignment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApplicationsAssignment.Application.Services
{
    public class AssignmentsService : IAssignmentsService
    {

        private IAssignmentsRepository _assignmentsRepository;

        public AssignmentsService(IAssignmentsRepository assignmentsRepository)
        {
            _assignmentsRepository = assignmentsRepository;
        }

        public IQueryable<AssignmentViewmodel> GetAssignments()
        {
            var list = from p in _assignmentsRepository.GetAssignments()
                       select new AssignmentViewmodel()
                       {
                           Id = p.Id,
                           Name = p.Name,
                           Description = p.Description,
                           Deadline = p.Deadline
                       };

            return list;
        }
    }
}
