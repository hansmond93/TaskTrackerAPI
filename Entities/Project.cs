using Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Project: FullAuditedEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? IsRecurring { get; set; }
        public ProjectState Status { get; set; }
        public string ReasonForClosure { get; set; }
        public DateTime? ClosureDate { get; set; }


        public ICollection<Task> Tasks { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
    }

    public enum ProjectState
    {
        Active = 1,

        Closed = 2

    }
}
