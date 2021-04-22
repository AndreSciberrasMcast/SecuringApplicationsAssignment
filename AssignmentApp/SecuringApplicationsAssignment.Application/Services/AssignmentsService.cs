using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;
using SecuringApplicationsAssignment.Data.Repositories;
using SecuringApplicationsAssignment.Domain.Interfaces;
using SecuringApplicationsAssignment.Domain.Models;
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

        public void AddAssignment(AssignmentViewmodel assignment)
        {
            Assignment newAssignment = new Assignment()
            {
                Name = assignment.Name,
                Description = assignment.Description,
                Deadline = assignment.Deadline
            };

            _assignmentsRepository.AddAssignment(newAssignment);
        }

        public void DeleteAssignment(Guid id)
        {
            var toDelete = _assignmentsRepository.GetAssignment(id);

            if (toDelete != null)
                _assignmentsRepository.DeleteAssignment(toDelete);
        }

        public AssignmentViewmodel GetAssignment(Guid id)
        {
            var data = _assignmentsRepository.GetAssignment(id);
            AssignmentViewmodel assignment = new AssignmentViewmodel();
            assignment.Id = data.Id;
            assignment.Name = data.Name;
            assignment.Description = data.Description;
            assignment.Deadline = data.Deadline;
            return assignment;
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
