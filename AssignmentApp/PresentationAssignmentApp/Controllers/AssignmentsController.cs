using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationAssignmentApp.Models;
using SecuringApplicationsAssignment.Application.Interfaces;
using SecuringApplicationsAssignment.Application.ViewModels;

namespace PresentationAssignmentApp.Controllers
{
    public class AssignmentsController : Controller
    {
        private IAssignmentsService _assignmentsService;

        public AssignmentsController(IAssignmentsService assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        [Authorize]
        public IActionResult Index()
        {
            
            var list = _assignmentsService.GetAssignments();   
            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult SubmitAssignment(Guid id)
        {
            var assignment = _assignmentsService.GetAssignment(id);
            ViewBag.Assignment = assignment;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitAssignment(AddSubmissionViewModel model)
        { 

            if (model.File != null)
            {
                Stream stream = model.File.OpenReadStream();
                int firstByte = stream.ReadByte();
                int secondByte = stream.ReadByte();
                int thirdByte = stream.ReadByte();
                int fourthByte = stream.ReadByte();
                stream.Position = 0;
                

                //pdf file signature 25 50 44
                if (firstByte == 37 && secondByte == 80 && thirdByte == 68 && fourthByte == 70 && Path.GetExtension(model.File.FileName) == ".pdf")
                {
                    using(FileStream fs = new FileStream(@"path", FileMode.CreateNew, FileAccess.Write))
                    {
                        stream.CopyTo(fs);
                    }
                   
                        //continue to uploading the file
                    //string absolutePath = _host.WebRootPath + @"\images\";
                    string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                    //System.IO.File.WriteAllBytes(absolutePath + uniqueName, ms.ToArray());

                    //Accept File
                }
                else
                {
                    ModelState.AddModelError("model", "File is not valid. Only PDF allowed.");
                    return View();
                }


            }
            else
            {
                ModelState.AddModelError("model", "Please upload your file");
                return View();
            }

            return View();
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
        public IActionResult Create(AssignmentViewmodel data)
        {
            try
            {
                _assignmentsService.AddAssignment(data);
                ViewData["feedback"] = "Assignment added";
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
                ViewData["feedback"] = "Assignment deleted";
            }catch(Exception ex)
            {
                ViewData["warning"] = "Error deleting assignment";
            }
            

            return RedirectToAction("Index");
        }
    }
}
