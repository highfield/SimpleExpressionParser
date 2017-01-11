using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    internal static class Parser
    {

        internal static XToken ResolveParensPair(IEnumerator<XToken> iter)
        {
            var list = new List<XToken>();
            while (iter.MoveNext())
            {
                XToken token = iter.Current;
                if (token is XTokenLParen)
                {
                    token = ResolveParensPair(iter);
                }
                else if (token is XTokenRParen)
                {
                    break;
                }
                list.Add(token);
            }
            return new XTokenSubTree(list);
        }


        internal static XTreeNodeBase BuildTree(XToken token)
        {
            if (token is XTokenSubTree)
            {
                var source = token.Data as IReadOnlyList<XToken>;
                return BuildTree(source, 0, source.Count - 1);
            }
            else if (token.Arity == 0)
            {
                return new XTreeNodeTerminal(token);
            }
            else
            {
                throw new XParserException($"Unexpected token: {token}");
            }
        }


        private static XTreeNodeBase BuildTree(
            IReadOnlyList<XToken> source,
            int ixa,
            int ixb
            )
        {
            if (ixa > ixb)
            {
                throw new XParserException("Empty expression.");
            }
            else if (ixa == ixb)
            {
                return BuildTree(source[ixa]);
            }

            for (int prio = 1; prio < 16; prio++)
            {
                for (int i = ixa; i <= ixb; i++)
                {
                    XToken token = source[i];
                    if (token.Prio == prio)
                    {
                        switch (token.Arity)
                        {
                            case 1:
                                return new XTreeNodeUnary(
                                    token,
                                    BuildTree(source, i + 1, ixb)
                                    );

                            case 2:
                                return new XTreeNodeBinary(
                                    token,
                                    BuildTree(source, ixa, i - 1),
                                    BuildTree(source, i + 1, ixb)
                                    );

                            default:
                                throw new XParserException($"Invalid operator type: {token}");
                        }
                    }
                }
            }

            throw new XParserException("Missing operator.");
        }

    }
}
