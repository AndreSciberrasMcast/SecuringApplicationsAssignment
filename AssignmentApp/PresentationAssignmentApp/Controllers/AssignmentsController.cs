using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecuringApplicationsAssignment.Application.Interfaces;

namespace PresentationAssignmentApp.Controllers
{
    public class AssignmentsController : Controller
    {
        private IAssignmentsService _assignmentsService;

        public AssignmentsController(IAssignmentsService assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        public IActionResult Index()
        {
            
            var list = _assignmentsService.GetAssignments();   
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
