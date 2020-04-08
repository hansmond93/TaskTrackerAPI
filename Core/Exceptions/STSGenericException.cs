using System;

namespace Core.Exceptions
{
    [Serializable]
    public class STSGenericException : Exception
    {
        public string ErrorCode { get; set; }

        public STSGenericException(string message) : base(message)
        { }

        public STSGenericException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}