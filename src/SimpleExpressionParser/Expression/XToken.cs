using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public abstract class XToken
    {
        protected XToken() : this(0, 0, null) { }

        protected XToken(object data) : this(0, 0, data) { }

        protected XToken(
            int arity,
            int prio,
            object data
            )
        {
            this.Arity = arity;
            this.Prio = prio;
            this.Data = data;
        }

        public int Arity { get; }
        public int Prio { get; }
        public object Data { get; }


        public abstract SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc);


        public override string ToString()
        {
            return $"{this.GetType().Name}: {this.Data}";
        }
    }
}
