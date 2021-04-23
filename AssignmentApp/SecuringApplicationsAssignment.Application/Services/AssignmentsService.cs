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

        private readonly IAssignmentsRepository _assignmentsRepository;
        private readonly IMembersRepository _membersRepository;
        private readonly IMembersService _membersService;
        private readonly ISubmissionsRepository _submissionsRepository;

        public AssignmentsService(IAssignmentsRepository assignmentsRepository, IMembersRepository membersRepository, ISubmissionsRepository submissionsRepository, IMembersService membersService)
        {
            _assignmentsRepository = assignmentsRepository;
            _membersRepository = membersRepository;
            _submissionsRepository = submissionsRepository;
            _membersService = membersService;
        }

        public void AddAssignment(AssignmentViewmodel assignment)
        {
            Assignment newAssignment = new Assignment()
            {
                Name = assignment.Name,
                Description = assignment.Description,
                Deadline = assignment.Deadline,
                MemberEmail = assignment.Member.Email
            };

            _assignmentsRepository.AddAssignment(newAssignment);
        }

        public void AddSubmission(SubmissionViewModel submission)
        {
            Submission sub = new Submission();
            sub.Assignment = _assignmentsRepository.GetAssignment(submission.Assignment.Id);
            sub.Member = _membersRepository.GetMember(submission.Member.Email);
            sub.FilePath = submission.FilePath;
            _submissionsRepository.AddSubmission(sub);
        }

        public SubmissionViewModel GetSubmission(Guid assignmentId, string studentEmail)
        {
            SubmissionViewModel submission = _submissionsRepository.GetSubmission(assignmentId, studentEmail);
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
            assignment.Member = _membersService.GetMember(data.MemberEmail);
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
                           Deadline = p.Deadline,
                           Member = new MemberViewModel() { Email = p.Member.Email, FirstName = p.Member.FirstName, LastName = p.Member.LastName }
                       };

            return list;
        }
    }
}
