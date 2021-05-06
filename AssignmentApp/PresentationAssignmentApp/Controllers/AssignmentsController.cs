using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AssignmentsController> _logger;

        private readonly IMembersService _membersService;

        public AssignmentsController(IAssignmentsService assignmentsService, IMembersService membersService, IWebHostEnvironment host, ILogger<AssignmentsController> logger)
        {
            _assignmentsService = assignmentsService;
            _membersService = membersService;
            _host = host;
            _logger = logger;
        }


        [Authorize]
        public IActionResult Index()
        {

            var list = _assignmentsService.GetAssignments();
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
                    SubmissionViewModel submission = new SubmissionViewModel();
                    submission.Member = _membersService.GetMember(User.Identity.Name);

                    Tuple<byte[], byte[]> keys = CryptographicHelper.GenerateKeys();


                    MemberViewModel teacher = _membersService.GetMember(submission.Member.TeacherEmail);

                    string encryptedKey = Convert.ToBase64String(CryptographicHelper.AsymmetricEncrypt(keys.Item1, teacher.PublicKey));

                    string encryptedIv = Convert.ToBase64String(CryptographicHelper.AsymmetricEncrypt(keys.Item2, teacher.PublicKey));


                    submission.SymmetricKey = encryptedKey;
                    submission.SymmetricIV = encryptedIv;

                    submission.Assignment = _assignmentsService.GetAssignment(assignment.Id);


                    string absolutePath = _host.WebRootPath + @"\..\ProtectedFiles\";
                    string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        ms.Position = 0;

                        submission.FileHash = Convert.ToBase64String(CryptographicHelper.Hash(ms.ToArray()));

                        var signature = CryptographicHelper.GenerateSignature(Convert.FromBase64String(submission.FileHash), submission.Member.PrivateKey);

                        submission.Signature = Convert.ToBase64String(CryptographicHelper.SymmetricEncrypt(
                            signature,
                            keys.Item1, keys.Item2));


                        System.IO.File.WriteAllBytes(absolutePath + uniqueName,
                            CryptographicHelper.SymmetricEncrypt(
                                    ms.ToArray(),
                                    keys.Item1,
                                    keys.Item2
                            )
                        );
                    }

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

            if (User.IsInRole("Teacher"))
            {
                var checks = CheckForAuthandInt(decryptedId);

                if(checks.Item2 == false)
                {
                    ViewData["isAuthentic"] = 0;
                }
            }

            var comments = _assignmentsService.GetComments(decryptedId);
             
            ViewBag.Comments = comments;
            return View(submission);
        }

        public Tuple<bool, bool> CheckForAuthandInt(Guid id){

            bool isDistinct = true;
            bool isVerified = true;

            SubmissionViewModel submission = _assignmentsService.GetSubmission(id);

            var submissions = _assignmentsService.GetSubmissions(submission.Assignment.Id);

            MemberViewModel teacher = _membersService.GetMember(submission.Member.TeacherEmail);


            byte[] key = CryptographicHelper.AsymmetricDecrypt(
                Convert.FromBase64String(submission.SymmetricKey), teacher.PrivateKey);

            byte[] iv = CryptographicHelper.AsymmetricDecrypt(
                 Convert.FromBase64String(submission.SymmetricIV), teacher.PrivateKey);

            foreach (SubmissionViewModel sub in submissions)
            {
                if (sub.FileHash == submission.FileHash && sub.Member.Email != submission.Member.Email)
                {
                    TempData["warning"] += "Assignment is identical to " + sub.Member.Email + "'s assignment!\n";
                    isDistinct = false;
                }
            }

            if (!CryptographicHelper.VerifySignature(
                        CryptographicHelper.SymmetricDecrypt(
                            Convert.FromBase64String(submission.Signature), key, iv),
                        CryptographicHelper.Hash(GetDecryptedAssignment(id)),
                        submission.Member.PublicKey))
            {
                isVerified = false;
            }

            return new Tuple<bool, bool>(isDistinct, isVerified);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]  
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
        [ValidateUserActionFilter]
        public IActionResult ViewFile(string id) 
        {
            Guid decryptedId = Guid.Parse(HttpUtility.UrlDecode(CryptographicHelper.SymmetricDecrypt(id)));

            byte[] decryptedAssignment = GetDecryptedAssignment(decryptedId);

            IPHostEntry iphostinfo = Dns.GetHostEntry(Dns.GetHostName());

            string ipaddress = Convert.ToString(iphostinfo.AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));


            if (User.IsInRole("Teacher"))
            {
                _logger.LogInformation("Teacher " + User.Identity.Name + " on IP " + ipaddress + " accessed file of submission " + decryptedId + " on " + DateTime.Now);
            }else if (User.IsInRole("Student")) {
                _logger.LogInformation("Student " + User.Identity.Name + " on IP " + ipaddress + " accessed file of submission " + decryptedId + " on " + DateTime.Now);
            }

            return File(decryptedAssignment, "application/pdf");
        }
       
        public byte[] GetDecryptedAssignment(Guid submissionId)
        {
            SubmissionViewModel submission = _assignmentsService.GetSubmission(submissionId);

            FileStream fs = new FileStream(submission.FilePath, FileMode.Open, FileAccess.Read);

            MemberViewModel teacher = _membersService.GetMember(submission.Member.TeacherEmail);

            byte[] key = CryptographicHelper.AsymmetricDecrypt(
                Convert.FromBase64String(submission.SymmetricKey), teacher.PrivateKey);

            byte[] iv = CryptographicHelper.AsymmetricDecrypt(
                 Convert.FromBase64String(submission.SymmetricIV), teacher.PrivateKey);


            MemoryStream ms = new MemoryStream();

            fs.CopyTo(ms);

            byte[] encryptedAssignment = ms.ToArray();

            byte[] decryptedAssignment = CryptographicHelper.SymmetricDecrypt(
                encryptedAssignment,
                key, iv);

            return decryptedAssignment;
        }

        public ILogger GetLogger()
        {
            return _logger;
        }

    }

}
