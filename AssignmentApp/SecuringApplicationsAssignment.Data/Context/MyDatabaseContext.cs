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

        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assignment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Comment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Comment>().HasOne(X => X.Submission).WithMany().OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Submission>()
                .HasOne(z => z.Assignment)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Submission>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            //modelBuilder.Entity<Member>().HasOne(x => x.Email).WithOne().
        }
    }
}
