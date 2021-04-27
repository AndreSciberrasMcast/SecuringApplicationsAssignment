using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationAssignmentApp.ActionFilters;
using PresentationAssignmentApp.Helpers;
using PresentationAssignmentApp.Models;
using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;

namespace PresentationAssignmentApp.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService _assignmentsService;
        private readonly IWebHostEnvironment _host;
        private readonly IMembersService _membersService;

        public AssignmentsController(IAssignmentsService assignmentsService, IMembersService membersService, IWebHostEnvironment host)
        {
            _assignmentsService = assignmentsService;
            _membersService = membersService;
            _host = host;
        }

        [Authorize]
        public IActionResult Index()
        {

            var list = _assignmentsService.GetAssignments();

            //AssignmentViewmodel assignment = _assignmentsService.GetAssignment(new Guid("F73ECF10-167C-4201-B52D-4EDC7776ED36"));


            List <AssignmentViewmodel> assignments = new List<AssignmentViewmodel>();

            if (User.IsInRole("Student"))
            {
                foreach (AssignmentViewmodel a in list)
                {
                    string assignmentIssuer = a.Member.Email;
                    string studentsTeacher = _membersService.GetMember(User.Identity.Name).TeacherEmail;

                    if (a.Member.Email.Equals(_membersService.GetMember(User.Identity.Name).TeacherEmail))
                    {
                        assignments.Add(a);
                    }
                }

                var submissions = _assignmentsService.GetSubmissions(User.Identity.Name);

                ViewBag.Submissions = submissions;
                return View(assignments);
            }

            return View(list);
            
        }


        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult SubmitAssignment(string id)
        {
            Guid decryptedId = Guid.Parse(CryptographicHelper.SymmetricDecrypt(id));

            CookieOptions cookieOptions = new CookieOptions();
            Response.Cookies.Append("Assignment", id.ToString(), cookieOptions);
            var assignment = _assignmentsService.GetAssignment(decryptedId);
            
            ViewBag.Assignment = assignment;

            return View();
        }
        

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitAssignment(IFormFile file)
        {
            var assignment = _assignmentsService.GetAssignment(Guid.Parse(Request.Cookies["Assignment"]));

            ViewBag.Assignment = assignment;


            if (file != null)
            {
                Stream stream = file.OpenReadStream();
                int firstByte = stream.ReadByte();
                int secondByte = stream.ReadByte();
                int thirdByte = stream.ReadByte();
                int fourthByte = stream.ReadByte();
                stream.Position = 0;


                //If the file passes the following check, a submission is created with user credentials
                if (firstByte == 37 && secondByte == 80 && thirdByte == 68 && fourthByte == 70 && Path.GetExtension(file.FileName) == ".pdf")
                {
                    string absolutePath = _host.WebRootPath + @"\..\ProtectedFiles\";
                    string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    ms.Position = 0;


                    System.IO.File.WriteAllBytes(absolutePath + uniqueName, ms.ToArray());

                    //Create a submission to be save to db
                    SubmissionViewModel submission = new SubmissionViewModel();
                    submission.Member = _membersService.GetMember(User.Identity.Name);
                    submission.Assignment = _assignmentsService.GetAssignment(Guid.Parse(Request.Cookies["Assignment"]));
                    
                    //Maybe encrypt
                    submission.FilePath = absolutePath;
                    _assignmentsService.AddSubmission(submission);

                    TempData["info"] = "File accepted";

                    return RedirectToAction("index");

                }
                else
                {
                    TempData["warning"] = "File is not valid, only PDF allowed";
                    return View();
                }
            }
            else
            {
                TempData["warning"] = "Please upload a file";

                return View();
            }
        }


        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            //Implement validation for deadline 
            return View();
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AssignmentViewmodel data)
        {
            try
            {
                //data.Member.Email = "scibby98@gmail.com";
                if(data.Name != null && data.Description != null && data.Deadline > DateTime.Now)
                {
                    data.Member = _membersService.GetMember(User.Identity.Name);
                    _assignmentsService.AddAssignment(data);
                    ViewData["info"] = "Assignment added";
                }
                else
                {
                    return View();
                }
                
            }
            catch(Exception ex)
            {
                ViewData["warning"] = "Assignment was not added!";

            }

            var list = _assignmentsService.GetAssignments();

            return View("Index", list);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Delete(Guid id)
        {

            try
            {
                _assignmentsService.DeleteAssignment(id);
                ViewData["info"] = "Assignment deleted";
            }catch(Exception ex)
            {
                ViewData["warning"] = "Error deleting assignment";
            }
            

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        [ValidateUserActionFilter]
        public IActionResult ViewSubmission(string id)
        {
            Guid decryptedId = Guid.Parse(CryptographicHelper.SymmetricDecrypt(id));

            ViewSubmissionViewModel submission = new ViewSubmissionViewModel();
            submission.Submission = _assignmentsService.GetSubmission(decryptedId);
           // CookieOptions cookieOptions = new CookieOptions();
           // Response.Cookies.Append("Submission", submission.Submission.Id.ToString(), cookieOptions);

            var comments = _assignmentsService.GetComments(decryptedId);

            ViewBag.Comments = comments;
            return View(submission);
        }

        [HttpPost]
        [Authorize]
        public IActionResult ViewSubmission()
        {
            //var comments = _assignmentsService.GetComments(Guid.Parse(Request.Cookies["Submission"]));
            return View();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult ViewSubmissions(Guid assigmmentId)
        {
            var list = _assignmentsService.GetSubmissions(assigmmentId);
            return View(list);
        }

        [Authorize]
        [CommentValidation]
        public IActionResult AddComment(ViewSubmissionViewModel data)
        {

            CommentViewModel comment = new CommentViewModel();
            comment.Data = data.Comment.Data;
            comment.Member = _membersService.GetMember(User.Identity.Name);
            comment.Submission = _assignmentsService.GetSubmission(data.Submission.Id);
            comment.Time = DateTime.Now;
            _assignmentsService.AddComment(comment);

            ViewSubmissionViewModel toReturn = new ViewSubmissionViewModel();
            toReturn.Submission = comment.Submission;
            
            var comments = _assignmentsService.GetComments(toReturn.Submission.Id);
            ViewBag.Comments = comments;

            return View("ViewSubmission", toReturn);
        }

    }

}
