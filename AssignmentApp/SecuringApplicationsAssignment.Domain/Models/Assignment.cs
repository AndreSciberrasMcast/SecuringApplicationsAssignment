using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Models
{
    public class Assignment
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

      //  [Required]
        //Teacher ID
       // public User User { get; set; }
    }
}
