using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public sealed class XTokenLParen : XToken { }


    public sealed class XTokenRParen : XToken { }


    internal sealed class XTokenSubTree : XToken
    {
        public XTokenSubTree(IReadOnlyList<XToken> list) : base(list) { }
    }
}
