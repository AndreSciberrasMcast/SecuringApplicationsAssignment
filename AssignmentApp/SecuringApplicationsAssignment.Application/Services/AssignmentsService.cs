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

        /*
        public SubmissionViewModel GetSubmission(Guid assignmentId, string studentEmail)
        { 
            var data = _submissionsRepository.GetSubmission(assignmentId, studentEmail);

            MemberViewModel member = new MemberViewModel();
            member.Email = data.Member.Email;
            member.FirstName = data.Member.FirstName;
            member.LastName = data.Member.LastName;
            member.TeacherEmail = data.Member.TeacherEmail;

            AssignmentViewmodel assignment = new AssignmentViewmodel();
            assignment.Id = data.Assignment.Id;
            assignment.Name = data.Assignment.Name;
            assignment.Description = data.Assignment.Description;
            assignment.Deadline = data.Assignment.Deadline;
            

            SubmissionViewModel submission = new SubmissionViewModel();
            submission.Member = member;
            submission.FilePath = data.FilePath;
            submission.Assignment = assignment;
            
            return submission;
        }*/

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

        public void AddSubmission(SubmissionViewModel submission)
        {
            Submission sub = new Submission();
            sub.Assignment = _assignmentsRepository.GetAssignment(submission.Assignment.Id);
            sub.Member = _membersRepository.GetMember(submission.Member.Email);
            sub.FilePath = submission.FilePath;
            _submissionsRepository.AddSubmission(sub);
        }

        public IQueryable<SubmissionViewModel> GetSubmissions(string email)
        {
            var list = from p in _submissionsRepository.GetSubmissionsByStudent(email)
                       select new SubmissionViewModel()
                       {
                           Id = p.Id,
                           FilePath = p.FilePath,
                           Assignment = new AssignmentViewmodel() { Id = p.Assignment.Id, Name = p.Assignment.Name, Description = p.Assignment.Description, Deadline = p.Assignment.Deadline },
                           Member = new MemberViewModel() { Email = p.Member.Email, FirstName = p.Member.LastName, TeacherEmail = p.Member.TeacherEmail }
                       };
            return list;
        }

        public IQueryable<SubmissionViewModel> GetSubmissions(Guid assignmentId)
        {
            var list = from p in _submissionsRepository.GetSubmissionsForASsignment(assignmentId)
                       select new SubmissionViewModel()
                       {
                           Id = p.Id,
                           FilePath = p.FilePath,
                           Assignment = new AssignmentViewmodel() { Id = p.Assignment.Id, Deadline = p.Assignment.Deadline, Description = p.Assignment.Description, Name = p.Assignment.Name },
                           Member = new MemberViewModel() { Email = p.Member.Email, FirstName = p.Member.FirstName, LastName = p.Member.LastName, TeacherEmail = p.Member.TeacherEmail}
                       };

            return list;
          
        }

        public SubmissionViewModel GetSubmission(Guid assignmentId, string email)
        {
            
            Submission sub = _submissionsRepository.GetSubmission(assignmentId, email);
            SubmissionViewModel submission = new SubmissionViewModel();
            submission.Id = sub.Id;
            submission.FilePath = sub.FilePath;
            submission.Assignment = GetAssignment(sub.AssignmentId);
            submission.Member = _membersService.GetMember(sub.MemberEmail);
            return submission;

        }

        public SubmissionViewModel GetSubmission(Guid id)
        {
            Submission sub = _submissionsRepository.GetSubmission(id);

            SubmissionViewModel submission = new SubmissionViewModel();
            submission.Id = sub.Id;
            submission.FilePath = sub.FilePath;
            submission.Member = _membersService.GetMember(sub.MemberEmail);
            submission.Assignment = GetAssignment(sub.AssignmentId);
            return submission;
        }

        public IQueryable<CommentViewModel> GetComments(Guid id)
        {
            var list = from p in _submissionsRepository.GetComments(id)
                       select new CommentViewModel()
                       {
                           Id = p.Id,
                           Data = p.Data,
                           Time = p.Time,
                           Submission = GetSubmission(id),
                           Member = _membersService.GetMember(p.MemberEmail)
                       };

            return list;
        }

        public void AddComment(CommentViewModel comment)
        {
            Comment comm = new Comment();
            comm.Data = comment.Data;
            comm.Time = comment.Time;
            comm.MemberEmail = comment.Member.Email;
            comm.SubmissionFk = comment.Submission.Id;
            _submissionsRepository.AddComment(comm);
        }

       
    }
}
