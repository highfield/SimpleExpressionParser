using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public abstract class TreeNodeBase
    {
        protected TreeNodeBase(XToken token)
        {
            this.Token = token;
        }

        public XToken Token { get; }

        public abstract IEnumerable<TreeNodeBase> GetChildren();

        public abstract SolverResult Resolve(ISolverContext context);
    }


    public sealed class TreeNodeTerminal : TreeNodeBase
    {
        public TreeNodeTerminal(XToken token) : base(token) { }

        public override IEnumerable<TreeNodeBase> GetChildren()
        {
            yield break;
        }

        public override SolverResult Resolve(ISolverContext context)
        {
            return this.Token.Resolve(context, null, null, null);
        }
    }


    public sealed class TreeNodeUnary : TreeNodeBase
    {
        public TreeNodeUnary(XToken token, TreeNodeBase child) : base(token)
        {
            this.Child = child;
        }

        public TreeNodeBase Child { get; }

        public override IEnumerable<TreeNodeBase> GetChildren()
        {
            yield return this.Child;
        }

        public override SolverResult Resolve(ISolverContext context)
        {
            return this.Token.Resolve(context, this.Child, null, null);
        }
    }


    public sealed class TreeNodeBinary : TreeNodeBase
    {
        public TreeNodeBinary(XToken token, TreeNodeBase left, TreeNodeBase right) : base(token)
        {
            this.LeftChild = left;
            this.RightChild = right;
        }

        public TreeNodeBase LeftChild { get; }
        public TreeNodeBase RightChild { get; }

        public override IEnumerable<TreeNodeBase> GetChildren()
        {
            yield return this.LeftChild;
            yield return this.RightChild;
        }

        public override SolverResult Resolve(ISolverContext context)
        {
            return this.Token.Resolve(context, this.LeftChild, this.RightChild, null);
        }
    }
}
