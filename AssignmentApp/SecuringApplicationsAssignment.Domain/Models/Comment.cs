using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Models
{
    public class Comment
    {

        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Data { get; set; }

        //[Required]
        //User or Teacher
       // public User User { get; set; }

        [Required]
        public virtual Submission Submission { get; set; }

        [ForeignKey("Submission")]
        public Guid submissionId { get; set; }

        public Comment Reply { get; set; }
    }
}
