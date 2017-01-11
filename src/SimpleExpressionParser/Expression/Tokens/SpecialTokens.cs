using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public sealed class XTokenLParen : XToken
    {

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            throw new NotImplementedException();
        }
    }


    public sealed class XTokenRParen : XToken
    {

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            throw new NotImplementedException();
        }
    }


    internal sealed class XTokenSubTree : XToken
    {
        public XTokenSubTree(IReadOnlyList<XToken> list) : base(list) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            throw new NotImplementedException();
        }
    }
}
