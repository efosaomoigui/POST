using System;

namespace GIGLS.Infrastructure
{
    [Serializable]
    public class GenericException : Exception
    {
        public string ErrorCode { get; set; }

        public Exception exception { get; set; }

        public GenericException(string message) : base(message)
        { }

        public GenericException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public GenericException(string message, Exception ex) : base(message, ex)
        {
            exception = ex;
        }
    }
}