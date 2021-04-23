using SecuringApplicationsAssignment.Data.Context;
using SecuringApplicationsAssignment.Domain.Interfaces;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApplicationsAssignment.Data.Repositories
{
    public class MembersRepository : IMembersRepository
    {
        MyDatabaseContext _context;

        public MembersRepository(MyDatabaseContext context)
        {
            _context = context;
        }

        public void AddMember(Member m)
        {
            _context.Members.Add(m);
            _context.SaveChanges();
        }

        public Member GetMember(string email)
        {
            return  _context.Members.SingleOrDefault(x => x.Email == email);        }
        }
}
