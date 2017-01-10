using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public sealed class XTokenLParen : XToken
    {

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            throw new NotImplementedException();
        }
    }


    public sealed class XTokenRParen : XToken
    {

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            throw new NotImplementedException();
        }
    }


    internal sealed class XTokenSubTree : XToken
    {
        public XTokenSubTree(IReadOnlyList<XToken> list) : base(list) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            throw new NotImplementedException();
        }
    }
}
