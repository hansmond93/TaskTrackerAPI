using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModel
{
    public class ResetPasswordModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public int otp { get; set; }
    }
}
