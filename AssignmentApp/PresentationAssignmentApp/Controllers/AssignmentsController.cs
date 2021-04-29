using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            List <AssignmentViewmodel> assignments = new List<AssignmentViewmodel>();
            /*
            foreach(AssignmentViewmodel assignment in list)
            {
                assignment.EncryptedId = HttpUtility.UrlEncode(CryptographicHelper.SymmetricEncrypt(assignment.Id.ToString()));
                assignments.Add(assignment);
            }
            */

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
            Guid decryptedId = Guid.Parse(
                HttpUtility.UrlDecode(CryptographicHelper.SymmetricDecrypt(id)));

            CookieOptions cookieOptions = new CookieOptions();
            Response.Cookies.Append("Assignment", id, cookieOptions);
            var assignment = _assignmentsService.GetAssignment(decryptedId);
            
            ViewBag.Assignment = assignment;

            return View();
        }
        

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitAssignment(IFormFile file)
        {
            var assignment = _assignmentsService.GetAssignment(Guid.Parse(CryptographicHelper.SymmetricDecrypt(Request.Cookies["Assignment"])));

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


                    System.IO.File.WriteAllBytes(absolutePath + uniqueName, CryptographicHelper.SymmetricEncrypt(ms.ToArray()));

                    //Create a submission to be save to db
                    SubmissionViewModel submission = new SubmissionViewModel();
                    submission.Member = _membersService.GetMember(User.Identity.Name);
                    submission.Assignment = _assignmentsService.GetAssignment(assignment.Id);
                    
                    //Maybe encrypt
                    submission.FilePath = absolutePath + uniqueName;
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
        public IActionResult Delete(string id)
        {
            try
            {
                Guid decryptedId = Guid.Parse(
                    HttpUtility.UrlDecode(CryptographicHelper.SymmetricDecrypt(id)));
                _assignmentsService.DeleteAssignment(decryptedId);
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
            Guid decryptedId = Guid.Parse(
                HttpUtility.UrlDecode(
                    CryptographicHelper.SymmetricDecrypt(id)));

            ViewSubmissionViewModel submission = new ViewSubmissionViewModel();
            submission.Submission = _assignmentsService.GetSubmission(decryptedId);
        

            var comments = _assignmentsService.GetComments(decryptedId);

            ViewBag.Comments = comments;
            return View(submission);
        }

        [HttpPost]
        [Authorize]
        public IActionResult ViewSubmission()
        {
            
            return View();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult ViewSubmissions(string assigmmentId)
        {
            Guid decryptedAssignmentId = Guid.Parse(
                HttpUtility.UrlDecode(
                    CryptographicHelper.SymmetricDecrypt(assigmmentId)));

            var list = _assignmentsService.GetSubmissions(decryptedAssignmentId);
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


        [Authorize]
        public IActionResult ViewFile(string id)
        {
            string filepath = _assignmentsService.GetSubmission(Guid.Parse(HttpUtility.UrlDecode(CryptographicHelper.SymmetricDecrypt(id)))).FilePath;

            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            MemoryStream ms = new MemoryStream();

            fs.CopyTo(ms);

            byte[] encryptedAssignment = ms.ToArray();

            byte[] decryptedAssignment = CryptographicHelper.SymmetricDecrypt(encryptedAssignment);

     
            return File(decryptedAssignment, "application/pdf");
        }

        public async Task<IActionResult> DownloadFile(string id)
        {
            string filepath = _assignmentsService.GetSubmission(Guid.Parse(HttpUtility.UrlDecode(CryptographicHelper.SymmetricDecrypt(id)))).FilePath;

            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            MemoryStream ms = new MemoryStream();

            fs.CopyTo(ms);

            byte[] encryptedAssignment = ms.ToArray();

            byte[] decryptedAssignment = CryptographicHelper.SymmetricDecrypt(encryptedAssignment);


            return File(decryptedAssignment, "application/pdf");
        }
       

    }

}
