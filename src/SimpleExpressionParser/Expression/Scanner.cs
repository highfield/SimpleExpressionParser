using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    internal static class Scanner
    {

        internal static void Scan(Reader reader)
        {
            char ch;
            while (reader.TryPeek(out ch))
            {
                Token token = null;
                switch (ch)
                {
                    case '(':
                        reader.Tokens.Add(new Token(TokenType.LParen));
                        reader.MoveNext();
                        break;
                    case ')':
                        reader.Tokens.Add(new Token(TokenType.RParen));
                        reader.MoveNext();
                        break;
                    case '&': token = ScanTwins(reader, ch, TokenType.OpAnd); break;
                    case '|': token = ScanTwins(reader, ch, TokenType.OpOr); break;
                    case '!': token = ScanExclamationSign(reader, ch); break;
                    case '=': token = ScanEqualSign(reader, ch); break;
                    case '<': token = ScanLessSign(reader, ch); break;
                    case '>': token = ScanGreaterSign(reader, ch); break;
                    case '"': token = ScanString(reader, ch); break;
                    case '\'': token = ScanString(reader, ch); break;
                    case '/': token = ScanRegex(reader, ch); break;
                    case '_': token = ScanLiteral(reader, ch); break;
                    case '-': token = ScanNumber(reader, ch); break;
                    case '+': token = ScanNumber(reader, ch); break;
                    default:
                        if (char.IsWhiteSpace(ch))
                        {
                            reader.MoveNext();
                        }
                        else if (char.IsDigit(ch))
                        {
                            token = ScanNumber(reader, ch);
                        }
                        else if (char.IsLetter(ch))
                        {
                            token = ScanLiteral(reader, ch);
                        }
                        else
                        {
                            throw new ParserException($"Illegal character found: {ch}");
                        }
                        break;
                }

                if (token != null)
                {
                    reader.Tokens.Add(token);
                }
            }
        }


        private static Token ScanNumber(
            Reader reader,
            char head
            )
        {
            int ixold = reader.Index;
            if ("-+".IndexOf(head) >= 0)
            {
                reader.MoveNext();
            }

            int ixdp = reader.Index;
            char ch;
            while (reader.TryPeek(out ch))
            {
                if (char.IsDigit(ch))
                {
                    reader.MoveNext();
                }
                else if (ch == '.')
                {
                    if (ixdp < reader.Index)
                    {
                        ixdp = reader.Index;
                        reader.MoveNext();
                    }
                    else
                    {
                        throw new ParserException($"Illegal character found: {ch}");
                    }
                }
                else
                {
                    break;
                }
            }
            if (ch == '.')
            {
                throw new ParserException($"Illegal character found: {ch}");
            }

            var literal = new string(reader.Source, ixold, reader.Index - ixold);
            return new Token(TokenType.Number, double.Parse(literal));
        }


        private static Token ScanString(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();
            int ixold = reader.Index;
            char ch;
            while (reader.TryPeek(out ch) && ch != head) reader.MoveNext();
            var literal = new string(reader.Source, ixold, reader.Index - ixold);
            reader.MoveNext();

            return new Token(TokenType.String, literal);
        }


        private static Token ScanLiteral(
            Reader reader,
            char head
            )
        {
            Func<char, bool> predicate = _ => char.IsLetterOrDigit(_) || _ == '_';

            int ixold = reader.Index;
            char ch;
            while (reader.TryPeek(out ch) && predicate(ch)) reader.MoveNext();
            var literal = new string(reader.Source, ixold, reader.Index - ixold);

            switch (literal)
            {
                case "false": return new Token(TokenType.FalseLiteral, false);
                case "true": return new Token(TokenType.TrueLiteral, true);
                case "null": return new Token(TokenType.NullLiteral);
                case "match": return new Token(TokenType.OpMatch);
                default: return new Token(TokenType.Identifier, literal);
            }
        }


        private static Token ScanRegex(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();
            int ixold = reader.Index;
            char ch;
            while (reader.TryPeek(out ch) && ch != head) reader.MoveNext();
            var literal = new string(reader.Source, ixold, reader.Index - ixold);
            reader.MoveNext();

            //flags
            var flags = string.Empty;
            while (reader.TryPeek(out ch) && char.IsWhiteSpace(ch) == false)
            {
                if ("gi".IndexOf(ch) < 0)
                {
                    throw new ParserException($"Unsupported Regex match flag: {ch}");
                }
                flags += ch;
                reader.MoveNext();
            }

            var token = new Token(TokenType.MatchParam, literal);
            token.Param = flags;
            return token;
        }


        private static Token ScanTwins(
            Reader reader,
            char head,
            TokenType result
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) == false || ch != head)
            {
                throw new ParserException($"Illegal character found: {ch}");
            }

            reader.MoveNext();
            return new Token(result);
        }


        private static Token ScanExclamationSign(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) && ch == '=')
            {
                reader.MoveNext();
                return new Token(TokenType.OpNotEqual);
            }
            else
            {
                return new Token(TokenType.OpNot);
            }
        }


        private static Token ScanEqualSign(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch))
            {
                reader.MoveNext();
                switch (ch)
                {
                    case '=': return new Token(TokenType.OpEqual);
                }
            }
            throw new ParserException($"Illegal character found: {ch}");
        }


        private static Token ScanLessSign(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) && ch == '=')
            {
                reader.MoveNext();
                return new Token(TokenType.OpLessEqualThan);
            }
            else
            {
                return new Token(TokenType.OpLessThan);
            }
        }


        private static Token ScanGreaterSign(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) && ch == '=')
            {
                reader.MoveNext();
                return new Token(TokenType.OpGreaterEqualThan);
            }
            else
            {
                return new Token(TokenType.OpGreaterThan);
            }
        }

    }
}
