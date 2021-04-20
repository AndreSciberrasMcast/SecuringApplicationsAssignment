using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Models
{
    public class Comment
    {

        [Key]
        public Guid ID { get; set; }
        
        [Required]
        public string Data { get; set; }

        //[Required]
        //User or Teacher
       // public User User { get; set; }

        [Required]
        public Submission Submission { get; set; }
    }
}
