using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;

namespace PresentationAssignmentApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMembersService _membersService;

        public UsersController(IMembersService membersService)
        {
            _membersService = membersService;
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddStudent(MemberViewModel m)
        {
            //Generate random password
            //Send email to student
            m.TeacherEmail = User.Identity.Name;
            _membersService.AddMember(m);
            return RedirectToAction("Index", "Assignments");
        }
    }
}
