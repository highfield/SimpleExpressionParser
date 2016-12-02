using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    //https://msdn.microsoft.com/en-us/library/2bxt6kc4.aspx
    public enum TokenType
    {
        Number = 0x0000,
        String = 0x0001,
        MatchParam = 0x000F,
        FalseLiteral = 0x010,
        TrueLiteral = 0x0011,
        NullLiteral = 0x0012,
        Identifier = 0x0020,

        OpOr = 0x020200,
        OpAnd = 0x020300,

        OpEqual = 0x020700,
        OpNotEqual = 0x020701,
        OpLessThan = 0x020702,
        OpLessEqualThan = 0x020703,
        OpGreaterThan = 0x020704,
        OpGreaterEqualThan = 0x020705,
        OpMatch = 0x020706,

        OpNot = 0x010A00,

        LParen = 0x10000000,
        RParen = 0x10000001,

        zTokens = 0x20000000,
    }


    public sealed class Token
    {
        internal Token(
            TokenType type,
            object data = null
            )
        {
            this.Type = type;
            this.Data = data;
        }

        public readonly TokenType Type;
        public readonly object Data;
        public object Param { get; internal set; }

        public override string ToString()
        {
            return $"{this.Type}: {this.Data}";
        }
    }
}
