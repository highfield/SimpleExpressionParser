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

            Token partialTree = ResolveParensPair(reader.Tokens.GetEnumerator());

            return BuildTree(partialTree);
        }


        private static Token ResolveParensPair(IEnumerator<Token> iter)
        {
            var list = new List<Token>();
            while (iter.MoveNext())
            {
                Token token = iter.Current;
                if (token.Type == TokenType.LParen)
                {
                    token = ResolveParensPair(iter);
                }
                else if (token.Type == TokenType.RParen)
                {
                    break;
                }
                list.Add(token);
            }
            return new Token(TokenType.zTokens, list);
        }


        private static TreeNodeBase BuildTree(Token token)
        {
            if (token.Type == TokenType.zTokens)
            {
                var source = token.Data as IReadOnlyList<Token>;
                return BuildTree(source, 0, source.Count - 1);
            }
            else if (((int)token.Type & 0x0F0000) == 0)
            {
                return new TreeNodeTerminal() { Token = token };
            }
            else
            {
                throw new ParserException($"Unexpected token: {token}");
            }
        }


        private static TreeNodeBase BuildTree(
            IReadOnlyList<Token> source,
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

            for (int prio = 0x0100; prio < 0x1000; prio += 0x100)
            {
                for (int i = ixa; i <= ixb; i++)
                {
                    Token token = source[i];
                    int type = (int)token.Type;
                    if ((type & 0x0F00) == prio)
                    {
                        switch ((type & 0x0F0000) >> 16)
                        {
                            case 1:
                                return new TreeNodeUnary()
                                {
                                    Token = token,
                                    Child = BuildTree(source, i + 1, ixb)
                                };

                            case 2:
                                return new TreeNodeBinary()
                                {
                                    Token = token,
                                    LeftChild = BuildTree(source, ixa, i - 1),
                                    RightChild = BuildTree(source, i + 1, ixb)
                                };

                            default:
                                throw new ParserException($"Invalid operator type: {type:X8}");
                        }
                    }
                }
            }

            throw new ParserException("Missing operator.");
        }

    }
}
