using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;
using SecuringApplicationsAssignment.Domain.Interfaces;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Application.Services
{
    public class MembersService : IMembersService
    {
        private IMembersRepository _membersRepository;

        public MembersService(IMembersRepository membersRepository)
        {
            _membersRepository = membersRepository;
        }

        public void AddMember(MemberViewModel m)
        {
            Member student = new Member();
            student.FirstName = m.FirstName;
            student.LastName = m.LastName;
            student.Email = m.Email;
            student.PrivateKey = m.PrivateKey;
            student.PublicKey = m.PublicKey;
            
            if(m.TeacherEmail != null)
            {
                student.TeacherEmail = m.TeacherEmail;
            }

            _membersRepository.AddMember(student);
        }

        public MemberViewModel GetMember(string email)
        {
            MemberViewModel m = new MemberViewModel();
            var member = _membersRepository.GetMember(email);

            if(member != null)
            {
                m.Email = member.Email;
                m.FirstName = member.FirstName;
                m.LastName = member.LastName;
                m.PrivateKey = member.PrivateKey;
                m.PublicKey = member.PublicKey;
                
                m.TeacherEmail = member.TeacherEmail;
                return m;
            }
            return null;
        }
    }
}
