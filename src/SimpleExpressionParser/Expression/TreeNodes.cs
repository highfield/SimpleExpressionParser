using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public abstract class TreeNodeBase
    {
        protected TreeNodeBase() { }
        public Token Token { get; internal set; }

        public abstract IEnumerable<TreeNodeBase> GetChildren();
    }


    public class TreeNodeTerminal : TreeNodeBase
    {
        public override IEnumerable<TreeNodeBase> GetChildren()
        {
            yield break;
        }
    }


    public class TreeNodeUnary : TreeNodeBase
    {
        public TreeNodeBase Child { get; internal set; }

        public override IEnumerable<TreeNodeBase> GetChildren()
        {
            yield return this.Child;
        }
    }


    public class TreeNodeBinary : TreeNodeBase
    {
        public TreeNodeBase LeftChild { get; internal set; }
        public TreeNodeBase RightChild { get; internal set; }

        public override IEnumerable<TreeNodeBase> GetChildren()
        {
            yield return this.LeftChild;
            yield return this.RightChild;
        }
    }
}
