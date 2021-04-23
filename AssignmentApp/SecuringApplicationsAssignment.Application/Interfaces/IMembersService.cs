using SecuringApplicationsAssignment.Application.ViewModels;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Application.Interfaces
{
    public interface IMembersService
    {
        void AddMember(MemberViewModel m);

        MemberViewModel GetMember(string email);
    }
}
