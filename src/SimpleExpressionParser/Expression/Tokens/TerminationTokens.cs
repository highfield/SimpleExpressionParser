using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public sealed class XTokenNumber : XToken
    {
        public XTokenNumber(double value) : base(value) { }
    }


    public sealed class XTokenString : XToken
    {
        public XTokenString(string value) : base(value) { }
    }


    public sealed class XTokenMatchParam : XToken
    {
        public XTokenMatchParam(string value) : base(value) { }
        public string Flags { get; set; }
    }


    public sealed class XTokenBoolean : XToken
    {
        public XTokenBoolean(bool value) : base(value) { }
    }


    public sealed class XTokenNull : XToken { }


    public sealed class XTokenRefId : XToken
    {
        public XTokenRefId(string value) : base(value) { }
    }
}
