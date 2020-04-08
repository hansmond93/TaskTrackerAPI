using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModel
{
    public class GetProjectVM
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsRecurring { get; set; }

        public ProjectState Status { get; set; }
        public DateTime? ClosureDate { get; set; }
        public string ReasonForClosure { get; set; }



    }
}
