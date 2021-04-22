using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;
using SecuringApplicationsAssignment.Domain.Interfaces;
using SecuringApplicationsAssignment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Application.Services
{
    public class StudentsService : IStudentsService
    {
        private IStudentsRepository _studentsRepository;

        public StudentsService(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public void AddStudent(Student s)
        {
            Student student = new Student();
            student.FirstName = s.FirstName;
            student.LastName = s.LastName;

            _studentsRepository.AddStudent(student);
        }
    }
}
