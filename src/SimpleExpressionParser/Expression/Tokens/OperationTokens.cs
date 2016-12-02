using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public sealed class XTokenOperOr : XToken
    {
        public XTokenOperOr() : base(2, 2, null) { }
    }


    public sealed class XTokenOperAnd : XToken
    {
        public XTokenOperAnd() : base(2, 3, null) { }
    }


    public sealed class XTokenOperEqual : XToken
    {
        public XTokenOperEqual() : base(2, 7, null) { }
    }


    public sealed class XTokenOperNotEqual : XToken
    {
        public XTokenOperNotEqual() : base(2, 7, null) { }
    }


    public sealed class XTokenOperLessThan : XToken
    {
        public XTokenOperLessThan() : base(2, 7, null) { }
    }


    public sealed class XTokenOperLessOrEqualThan : XToken
    {
        public XTokenOperLessOrEqualThan() : base(2, 7, null) { }
    }


    public sealed class XTokenOperGreaterThan : XToken
    {
        public XTokenOperGreaterThan() : base(2, 7, null) { }
    }


    public sealed class XTokenOperGreaterOrEqualThan : XToken
    {
        public XTokenOperGreaterOrEqualThan() : base(2, 7, null) { }
    }


    public sealed class XTokenOperMatch : XToken
    {
        public XTokenOperMatch() : base(2, 7, null) { }
    }


    public sealed class XTokenOperNot : XToken
    {
        public XTokenOperNot() : base(1, 10, null) { }
    }

}
