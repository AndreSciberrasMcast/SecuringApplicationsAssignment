using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationAssignmentApp.Models;
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
        public IActionResult AddStudent(AddStudentModel model)
        {
            //Generate random password
            //Send email to student

            EmailModel em = new EmailModel();
            em.Email = "securingappsemailsender@gmail.com";
            em.To = model.MemberModel.Email;
            model.EmailModel = em;

            using(MailMessage mm = new MailMessage(model.EmailModel.Email, model.EmailModel.To))
            {
                mm.Subject = "Login Credentials";
                mm.Body = "You have been assigned an account. Your password is :";// + password;
                mm.IsBodyHtml = false;

                using(SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(model.EmailModel.Email, "74bf*XBG^0ga");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email sent";
                }
            }


            model.MemberModel.TeacherEmail = User.Identity.Name;
            _membersService.AddMember(model.MemberModel);
            return RedirectToAction("Index", "Assignments");
        }
    }
}
