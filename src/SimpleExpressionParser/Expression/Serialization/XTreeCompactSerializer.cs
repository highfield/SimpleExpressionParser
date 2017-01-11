using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public class XTreeCompactSerializer
        : XTreeSerializerBase<string>
    {
        public Dictionary<Type, Action<XToken, SerializationContext>> VisitorMap { get; } = new Dictionary<Type, Action<XToken, SerializationContext>>()
        {
            [typeof(XTokenNumber)] = SerializeTokenNumber,
            [typeof(XTokenString)] = SerializeTokenString,
            [typeof(XTokenMatchParam)] = SerializeTokenMatch,
            [typeof(XTokenBoolean)] = SerializeTokenBoolean,
            [typeof(XTokenNull)] = SerializeTokenNull,
            [typeof(XTokenRefId)] = SerializeTokenRefId,

            [typeof(XTokenOperOr)] = SerializeTokenOperOr,
            [typeof(XTokenOperAnd)] = SerializeTokenOperAnd,
            [typeof(XTokenOperEqual)] = SerializeTokenOperEqual,
            [typeof(XTokenOperNotEqual)] = SerializeTokenOperNotEqual,
            [typeof(XTokenOperLessThan)] = SerializeTokenOperLessThan,
            [typeof(XTokenOperLessOrEqualThan)] = SerializeTokenOperLessOrEqualThan,
            [typeof(XTokenOperGreaterThan)] = SerializeTokenOperGreaterThan,
            [typeof(XTokenOperGreaterOrEqualThan)] = SerializeTokenOperGreaterOrEqualThan,
            [typeof(XTokenOperMatch)] = SerializeTokenOperMatch,
            [typeof(XTokenOperNot)] = SerializeTokenOperNot,
        };


        public bool ShouldPad { get; set; }


        public override string Serialize(XTreeNodeBase xtree)
        {
            var ctx = new SerializationContext();
            ctx.AddSpaces = this.ShouldPad;
            this.Serialize(xtree, ctx, 0);
            return ctx.Builder.ToString();
        }


        private void Serialize(XTreeNodeBase xtree, SerializationContext ctx, int level)
        {
            var term = xtree as XTreeNodeTerminal;
            var unary = xtree as XTreeNodeUnary;
            var binary = xtree as XTreeNodeBinary;

            if (term != null)
            {
                this.VisitorMap[term.Token.GetType()](term.Token, ctx);
            }
            else if (unary != null)
            {
                this.VisitorMap[unary.Token.GetType()](unary.Token, ctx);
                this.Serialize(unary.Child, ctx, level + 1);
            }
            else if (binary != null)
            {
                if (level != 0) ctx.Builder.Append('(');
                ctx.LastIsAlphaNum = false;

                this.Serialize(binary.LeftChild, ctx, level + 1);
                if (ctx.AddSpaces)
                {
                    ctx.Builder.Append(' ');
                    ctx.LastIsAlphaNum = false;
                }
                this.VisitorMap[binary.Token.GetType()](binary.Token, ctx);
                if (ctx.AddSpaces)
                {
                    ctx.Builder.Append(' ');
                    ctx.LastIsAlphaNum = false;
                }
                this.Serialize(binary.RightChild, ctx, level + 1);

                if (level != 0)
                {
                    ctx.Builder.Append(')');
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        private static void SerializeTokenNumber(XToken token, SerializationContext ctx)
        {
            if (ctx.LastIsAlphaNum) ctx.Builder.Append(' ');
            var value = (double)token.Data;
            ctx.Builder.Append(value.ToString(CultureInfo.InvariantCulture));
            ctx.LastIsAlphaNum = true;
        }

        private static void SerializeTokenString(XToken token, SerializationContext ctx)
        {
            var value = (string)token.Data;
            ctx.Builder.Append('"' + value + '"');
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenMatch(XToken token, SerializationContext ctx)
        {
            var mtk = (XTokenMatchParam)token;
            ctx.Builder.Append($"/{mtk.Data}/{mtk.Flags}");
            ctx.LastIsAlphaNum = true;
        }

        private static void SerializeTokenBoolean(XToken token, SerializationContext ctx)
        {
            if (ctx.LastIsAlphaNum) ctx.Builder.Append(' ');
            var value = (bool)token.Data;
            ctx.Builder.Append(value ? "true" : "false");
            ctx.LastIsAlphaNum = true;
        }

        private static void SerializeTokenNull(XToken token, SerializationContext ctx)
        {
            if (ctx.LastIsAlphaNum) ctx.Builder.Append(' ');
            ctx.Builder.Append("null");
            ctx.LastIsAlphaNum = true;
        }

        private static void SerializeTokenRefId(XToken token, SerializationContext ctx)
        {
            if (ctx.LastIsAlphaNum) ctx.Builder.Append(' ');
            ctx.Builder.Append(token.Data);
            ctx.LastIsAlphaNum = true;
        }



        private static void SerializeTokenOperOr(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append("||");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperAnd(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append("&&");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperEqual(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append("==");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperNotEqual(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append("!=");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperLessThan(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append("<");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperLessOrEqualThan(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append("<=");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperGreaterThan(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append(">");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperGreaterOrEqualThan(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append(">=");
            ctx.LastIsAlphaNum = false;
        }

        private static void SerializeTokenOperMatch(XToken token, SerializationContext ctx)
        {
            if (ctx.LastIsAlphaNum) ctx.Builder.Append(' ');
            ctx.Builder.Append("match");
            ctx.LastIsAlphaNum = true;
        }

        private static void SerializeTokenOperNot(XToken token, SerializationContext ctx)
        {
            ctx.Builder.Append("!");
            ctx.LastIsAlphaNum = false;
        }


        public class SerializationContext
        {
            public StringBuilder Builder { get; } = new StringBuilder();
            public bool AddSpaces;

            public bool LastIsAlphaNum;
        }
    }
}
