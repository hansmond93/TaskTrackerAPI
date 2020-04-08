using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Messaging.Messages
{
    public static class NewAccountMessages
    {
        public static string NewSignUpMessage 
        { 
            get
            {
                return "Thanks for registering with TrackR, Please initiate a password reset to activate your account with a password";
            }
        }

        public static string OtpSentMessage
        {
            get
            {
                return "Use this one time password to initiate a password reset ";
            }
        }
    }
}
