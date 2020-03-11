using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_1.Exceptions
{
    class CustomException : Exception
    {
        public CustomException()
        {

        }
        public CustomException(string message) : base(message)
        {
            Console.WriteLine(message);
        }

        public CustomException(string message, Exception innerexception) : base(message, innerexception)
        {

        }
    }
}
