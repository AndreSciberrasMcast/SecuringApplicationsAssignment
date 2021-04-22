using System;
using System.Collections.Generic;
using System.Text;

namespace SecuringApplicationsAssignment.Application.ViewModels
{
    public class AssignmentViewmodel
    {
       
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }
    }
}
