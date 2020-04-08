using Microsoft.AspNetCore.Identity;
using System;

namespace Entities
{
    public class Role : IdentityRole<int>
    {
        public bool IsActive { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool IsDefaultRole { get; set; }
    }
}