﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SecuringApplicationsAssignment.Data.Context;

namespace SecuringApplicationsAssignment.Data.Migrations
{
    [DbContext(typeof(MyDatabaseContext))]
    [Migration("20210502101010_AddedSignature")]
    partial class AddedSignature
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SecuringApplicationsAssignment.Domain.Models.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MemberEmail");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("SecuringApplicationsAssignment.Domain.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("SubmissionFk")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MemberEmail");

                    b.HasIndex("SubmissionFk");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SecuringApplicationsAssignment.Domain.Models.Member", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrivateKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("SecuringApplicationsAssignment.Domain.Models.Submission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("AssignmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Signature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SymmetricIV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SymmetricKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId")
                        .IsUnique();

                    b.HasIndex("MemberEmail");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("SecuringApplicationsAssignment.Domain.Models.Assignment", b =>
                {
                    b.HasOne("SecuringApplicationsAssignment.Domain.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SecuringApplicationsAssignment.Domain.Models.Comment", b =>
                {
                    b.HasOne("SecuringApplicationsAssignment.Domain.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SecuringApplicationsAssignment.Domain.Models.Submission", "Submission")
                        .WithMany()
                        .HasForeignKey("SubmissionFk")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("SecuringApplicationsAssignment.Domain.Models.Submission", b =>
                {
                    b.HasOne("SecuringApplicationsAssignment.Domain.Models.Assignment", "Assignment")
                        .WithOne()
                        .HasForeignKey("SecuringApplicationsAssignment.Domain.Models.Submission", "AssignmentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SecuringApplicationsAssignment.Domain.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
