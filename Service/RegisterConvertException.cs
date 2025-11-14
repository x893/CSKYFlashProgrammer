using System;

namespace Service
{
    public class RegisterConvertException : Exception
    {
        public RegisterConvertException()
        {
        }

        public RegisterConvertException(string message)
          : base(message)
        {
        }

        public RegisterConvertException(string message, Exception inner)
          : base(message, inner)
        {
        }
    }
}
