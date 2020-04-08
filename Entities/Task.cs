using Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class Task : FullAuditedEntity<int>
    {
        public string Description { get; set; }
        public byte? Hours { get; set; }
        public byte? Minutes { get; set; }


        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }

    }
}
