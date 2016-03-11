using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InboundFileProcessor.Common
{
    public class FormatConversionException : Exception
    {

        public FormatConversionException()
        {
        }

        public FormatConversionException(string message): base(message)
        {
        }

        public FormatConversionException(string message, Exception inner): base(message, inner)
        {
        }


    }
}
