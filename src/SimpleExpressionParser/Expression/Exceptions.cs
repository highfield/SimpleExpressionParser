using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public class XParserException : Exception
    {
        public XParserException(string message) : base(message) { }
        public XParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
