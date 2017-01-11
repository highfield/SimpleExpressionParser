using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public abstract class XTreeNodeBase
    {
        protected XTreeNodeBase(XToken token)
        {
            this.Token = token;
        }

        public XToken Token { get; }

        public abstract IEnumerable<XTreeNodeBase> GetChildren();

        public abstract XSolverResult Resolve(IXSolverContext context);

        public static XTreeNodeBase Parse(string text)
        {
            var reader = new Reader(text ?? string.Empty);
            Scanner.Scan(reader);

            XToken partialTree = Parser.ResolveParensPair(reader.Tokens.GetEnumerator());

            return Parser.BuildTree(partialTree);
        }

    }


    public sealed class XTreeNodeTerminal : XTreeNodeBase
    {
        public XTreeNodeTerminal(XToken token) : base(token) { }

        public override IEnumerable<XTreeNodeBase> GetChildren()
        {
            yield break;
        }

        public override XSolverResult Resolve(IXSolverContext context)
        {
            return this.Token.Resolve(context, null, null, null);
        }
    }


    public sealed class XTreeNodeUnary : XTreeNodeBase
    {
        public XTreeNodeUnary(XToken token, XTreeNodeBase child) : base(token)
        {
            this.Child = child;
        }

        public XTreeNodeBase Child { get; }

        public override IEnumerable<XTreeNodeBase> GetChildren()
        {
            yield return this.Child;
        }

        public override XSolverResult Resolve(IXSolverContext context)
        {
            return this.Token.Resolve(context, this.Child, null, null);
        }
    }


    public sealed class XTreeNodeBinary : XTreeNodeBase
    {
        public XTreeNodeBinary(XToken token, XTreeNodeBase left, XTreeNodeBase right) : base(token)
        {
            this.LeftChild = left;
            this.RightChild = right;
        }

        public XTreeNodeBase LeftChild { get; }
        public XTreeNodeBase RightChild { get; }

        public override IEnumerable<XTreeNodeBase> GetChildren()
        {
            yield return this.LeftChild;
            yield return this.RightChild;
        }

        public override XSolverResult Resolve(IXSolverContext context)
        {
            return this.Token.Resolve(context, this.LeftChild, this.RightChild, null);
        }
    }
}
