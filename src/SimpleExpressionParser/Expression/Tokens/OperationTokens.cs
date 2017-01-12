using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public interface ITokenAssociative { }


    public sealed class XTokenOperOr : XToken, ITokenAssociative
    {
        public XTokenOperOr() : base(2, 2, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult result = na.Resolve(context);
            if (result.Error != null) return result;
            bool value = XSolverHelpers.AsBool(result.Data);
            if (value)
            {
                return XSolverResult.FromData(value);
            }

            result = nb.Resolve(context);
            if (result.Error != null) return result;
            value = XSolverHelpers.AsBool(result.Data);
            return XSolverResult.FromData(value);
        }
    }


    public sealed class XTokenOperAnd : XToken, ITokenAssociative
    {
        public XTokenOperAnd() : base(2, 3, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult result = na.Resolve(context);
            if (result.Error != null) return result;
            bool value = XSolverHelpers.AsBool(result.Data);
            if (value == false)
            {
                return XSolverResult.FromData(value);
            }

            result = nb.Resolve(context);
            if (result.Error != null) return result;
            value = XSolverHelpers.AsBool(result.Data);
            return XSolverResult.FromData(value);
        }
    }


    public sealed class XTokenOperEqual : XToken
    {
        public XTokenOperEqual() : base(2, 7, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            XSolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            bool value = XSolverHelpers.Match(sra.Data, srb.Data);
            return XSolverResult.FromData(value);
        }
    }


    public sealed class XTokenOperNotEqual : XToken
    {
        public XTokenOperNotEqual() : base(2, 7, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            XSolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            bool value = XSolverHelpers.Match(sra.Data, srb.Data);
            return XSolverResult.FromData(value == false);
        }
    }


    public sealed class XTokenOperLessThan : XToken
    {
        public XTokenOperLessThan() : base(2, 7, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            XSolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = XSolverHelpers.Compare(sra.Data, srb.Data);
            return XSolverResult.FromData(value < 0);
        }
    }


    public sealed class XTokenOperLessOrEqualThan : XToken
    {
        public XTokenOperLessOrEqualThan() : base(2, 7, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            XSolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = XSolverHelpers.Compare(sra.Data, srb.Data);
            return XSolverResult.FromData(value <= 0);
        }
    }


    public sealed class XTokenOperGreaterThan : XToken
    {
        public XTokenOperGreaterThan() : base(2, 7, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            XSolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = XSolverHelpers.Compare(sra.Data, srb.Data);
            return XSolverResult.FromData(value > 0);
        }
    }


    public sealed class XTokenOperGreaterOrEqualThan : XToken
    {
        public XTokenOperGreaterOrEqualThan() : base(2, 7, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            XSolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = XSolverHelpers.Compare(sra.Data, srb.Data);
            return XSolverResult.FromData(value >= 0);
        }
    }


    public sealed class XTokenOperMatch : XToken
    {
        public XTokenOperMatch() : base(2, 7, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            XSolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            var s = sra.Data as string;
            if (s == null) return XSolverResult.FromData(false);

            var re = srb.Data as Regex;
            if (re == null) return XSolverResult.FromData(false);

            Match match = re.Match(s);
            return XSolverResult.FromData(match.Success);
        }
    }


    public sealed class XTokenOperNot : XToken
    {
        public XTokenOperNot() : base(1, 10, null) { }

        public override XSolverResult Resolve(IXSolverContext context, XTreeNodeBase na, XTreeNodeBase nb, XTreeNodeBase nc)
        {
            XSolverResult result = na.Resolve(context);
            if (result.Error != null) return result;
            bool value = XSolverHelpers.AsBool(result.Data);
            return XSolverResult.FromData(value == false);
        }
    }

}
