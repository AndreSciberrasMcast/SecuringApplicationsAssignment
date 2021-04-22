using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationAssignmentApp.Models
{
    public class AddSubmissionViewModel
    {
        public IFormFile File { get; set; }
        public AddSubmissionViewModel Submission { get; set; }
    }
}
