using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationAssignmentApp.Data;
using PresentationAssignmentApp.Models;
using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;

namespace PresentationAssignmentApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMembersService _membersService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IMembersService membersService, UserManager<ApplicationUser> userManager)
        {
            _membersService = membersService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudent(AddStudentModel model)
        {

            if (!ModelState.IsValid)
            {
                TempData["warning"] = "Invalid";
                return View();
            }

            if(_membersService.GetMember(model.MemberModel.Email) != null)
            {
                TempData["warning"] = "Student already exists";
                return RedirectToAction("Index", "Assignments");
            }

            string randomPassword = GenerateRandomPassword();


            var user = new ApplicationUser { UserName = model.MemberModel.Email, Email = model.MemberModel.Email };

            var result = _userManager.CreateAsync(user, randomPassword);
            
            

            if (result.Result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Student");

                EmailModel em = new EmailModel();
                em.Email = "securingappsemailsender@gmail.com";
                em.To = model.MemberModel.Email;
                model.EmailModel = em;

                using (MailMessage mm = new MailMessage(model.EmailModel.Email, model.EmailModel.To))
                {
                    mm.Subject = "Login Credentials";
                    mm.Body = "You have been assigned an account. Username is: " + em.To + " and your password is : " + randomPassword;

                    mm.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient())
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

            }

            return RedirectToAction("Index", "Assignments");

        }

        private string GenerateRandomPassword()
        {
            List<int> numbers = new List<int>();

            Random r = new Random();
            
            for(int i = 0; i < 12; i++)
            {
                numbers.Add(r.Next(47, 122));
            }

            string password = "";

            foreach(int num in numbers)
            {
                password += (char)num;
            }

            password += "@$";

            return password;
        }
    }
}