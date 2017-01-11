using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public sealed class XTokenNumber : XToken
    {
        public XTokenNumber(double value) : base(value) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            return XSolverResult.FromData(this.Data);
        }
    }


    public sealed class XTokenString : XToken
    {
        public XTokenString(string value) : base(value) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            return XSolverResult.FromData(this.Data);
        }
    }


    public sealed class XTokenMatchParam : XToken
    {
        public XTokenMatchParam(string value) : base(value) { }
        public string Flags { get; set; }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            var pattern = this.Data as string;
            if (pattern == null) return XSolverResult.FromData(null);

            var flags = this.Flags ?? string.Empty;
            RegexOptions options = RegexOptions.None;
            if (flags.Contains('i')) options |= RegexOptions.IgnoreCase;

            var re = new Regex(pattern, options);
            return XSolverResult.FromData(re);
        }
    }


    public sealed class XTokenBoolean : XToken
    {
        public XTokenBoolean(bool value) : base(value) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            return XSolverResult.FromData(this.Data);
        }
    }


    public sealed class XTokenNull : XToken
    {

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            return XSolverResult.FromData(this.Data);
        }
    }


    public sealed class XTokenRefId : XToken
    {
        public XTokenRefId(string value) : base(value) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            if (context.ReferenceSolver == null) return XSolverResult.FromData(null);
            return context.ReferenceSolver.GetValue(this);
        }
    }
}
