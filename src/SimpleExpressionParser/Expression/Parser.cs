using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public static class Parser
    {
        public static TreeNodeBase Parse(string text)
        {
            var reader = new Reader(text ?? string.Empty);
            Scanner.Scan(reader);

            XToken partialTree = ResolveParensPair(reader.Tokens.GetEnumerator());

            return BuildTree(partialTree);
        }


        private static XToken ResolveParensPair(IEnumerator<XToken> iter)
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


        private static TreeNodeBase BuildTree(XToken token)
        {
            if (token is XTokenSubTree)
            {
                var source = token.Data as IReadOnlyList<XToken>;
                return BuildTree(source, 0, source.Count - 1);
            }
            else if (token.Arity == 0)
            {
                return new TreeNodeTerminal(token);
            }
            else
            {
                throw new ParserException($"Unexpected token: {token}");
            }
        }


        private static TreeNodeBase BuildTree(
            IReadOnlyList<XToken> source,
            int ixa,
            int ixb
            )
        {
            if (ixa > ixb)
            {
                throw new ParserException("Empty expression.");
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
                                return new TreeNodeUnary(
                                    token,
                                    BuildTree(source, i + 1, ixb)
                                    );

                            case 2:
                                return new TreeNodeBinary(
                                    token,
                                    BuildTree(source, ixa, i - 1),
                                    BuildTree(source, i + 1, ixb)
                                    );

                            default:
                                throw new ParserException($"Invalid operator type: {token}");
                        }
                    }
                }
            }

            throw new ParserException("Missing operator.");
        }

    }
}
