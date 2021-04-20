using Microsoft.EntityFrameworkCore;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Data.Context
{

    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Submission> Submissions { get; set; }
    }
}
