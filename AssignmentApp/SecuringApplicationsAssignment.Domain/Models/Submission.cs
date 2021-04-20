using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Models
{
    public class Submission
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Assignment Assignment { get; set; }

        public List<Comment> Comments { get; set; }

       // [Required]
        //Student ID
      //  public User User { get; set; }

    }
}
