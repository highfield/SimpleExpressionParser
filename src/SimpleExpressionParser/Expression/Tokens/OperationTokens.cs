using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public sealed class XTokenOperOr : XToken
    {
        public XTokenOperOr() : base(2, 2, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult result = na.Resolve(context);
            if (result.Error != null) return result;
            bool value = SolverHelpers.AsBool(result.Data);
            if (value)
            {
                return SolverResult.FromData(value);
            }

            result = na.Resolve(context);
            if (result.Error != null) return result;
            value = SolverHelpers.AsBool(result.Data);
            return SolverResult.FromData(value);
        }
    }


    public sealed class XTokenOperAnd : XToken
    {
        public XTokenOperAnd() : base(2, 3, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult result = na.Resolve(context);
            if (result.Error != null) return result;
            bool value = SolverHelpers.AsBool(result.Data);
            if (value == false)
            {
                return SolverResult.FromData(value);
            }

            result = na.Resolve(context);
            if (result.Error != null) return result;
            value = SolverHelpers.AsBool(result.Data);
            return SolverResult.FromData(value);
        }
    }


    public sealed class XTokenOperEqual : XToken
    {
        public XTokenOperEqual() : base(2, 7, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            SolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            bool value = SolverHelpers.Match(sra.Data, srb.Data);
            return SolverResult.FromData(value);
        }
    }


    public sealed class XTokenOperNotEqual : XToken
    {
        public XTokenOperNotEqual() : base(2, 7, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            SolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            bool value = SolverHelpers.Match(sra.Data, srb.Data);
            return SolverResult.FromData(value == false);
        }
    }


    public sealed class XTokenOperLessThan : XToken
    {
        public XTokenOperLessThan() : base(2, 7, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            SolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = SolverHelpers.Compare(sra.Data, srb.Data);
            return SolverResult.FromData(value < 0);
        }
    }


    public sealed class XTokenOperLessOrEqualThan : XToken
    {
        public XTokenOperLessOrEqualThan() : base(2, 7, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            SolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = SolverHelpers.Compare(sra.Data, srb.Data);
            return SolverResult.FromData(value <= 0);
        }
    }


    public sealed class XTokenOperGreaterThan : XToken
    {
        public XTokenOperGreaterThan() : base(2, 7, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            SolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = SolverHelpers.Compare(sra.Data, srb.Data);
            return SolverResult.FromData(value > 0);
        }
    }


    public sealed class XTokenOperGreaterOrEqualThan : XToken
    {
        public XTokenOperGreaterOrEqualThan() : base(2, 7, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            SolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            int value = SolverHelpers.Compare(sra.Data, srb.Data);
            return SolverResult.FromData(value >= 0);
        }
    }


    public sealed class XTokenOperMatch : XToken
    {
        public XTokenOperMatch() : base(2, 7, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult sra = na.Resolve(context);
            if (sra.Error != null) return sra;

            SolverResult srb = nb.Resolve(context);
            if (srb.Error != null) return srb;

            var s = sra.Data as string;
            if (s == null) return SolverResult.FromData(false);

            var re = srb.Data as Regex;
            if (re == null) return SolverResult.FromData(false);

            Match match = re.Match(s);
            return SolverResult.FromData(match.Success);
        }
    }


    public sealed class XTokenOperNot : XToken
    {
        public XTokenOperNot() : base(1, 10, null) { }

        public override SolverResult Resolve(ISolverContext context, TreeNodeBase na, TreeNodeBase nb, TreeNodeBase nc)
        {
            SolverResult result = na.Resolve(context);
            if (result.Error != null) return result;
            bool value = SolverHelpers.AsBool(result.Data);
            return SolverResult.FromData(value == false);
        }
    }

}
