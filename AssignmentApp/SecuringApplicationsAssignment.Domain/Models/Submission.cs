using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SecuringApplicationsAssignment.Domain.Models
{
    public class Submission
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public virtual Assignment Assignment { get; set; }

        [ForeignKey("Assignment")]
        public Guid AssignmentId { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string FileHash { get; set; }

        [Required]
        public string Signature { get; set; }

        [Required]
        public string SymmetricKey { get; set; }

        [Required]
        public string SymmetricIV { get; set; }

        [Required]
        public virtual Member Member { get; set; }

        [ForeignKey("Member")]
        public string MemberEmail { get; set; }

    }
}
