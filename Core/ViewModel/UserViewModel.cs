using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        #nullable enable
        public DateTime? Birthday { get; set; }
        public string? Designation { get; set; }
        public string? EmployeeID { get; set; }
        #nullable restore
        public string UserName { get; set; }

        public string FullName { 
            get
            {
                return FirstName + " " + LastName;
            } 
        }
    }
}
