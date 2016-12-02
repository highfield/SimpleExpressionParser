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
                XToken token = null;
                switch (ch)
                {
                    case '(':
                        reader.Tokens.Add(new XTokenLParen());
                        reader.MoveNext();
                        break;
                    case ')':
                        reader.Tokens.Add(new XTokenRParen());
                        reader.MoveNext();
                        break;
                    case '&': token = ScanTwins(reader, ch, new XTokenOperAnd()); break;
                    case '|': token = ScanTwins(reader, ch, new XTokenOperOr()); break;
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


        private static XToken ScanNumber(
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
            int count = 0;
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
                count++;
            }
            if (reader.Source[reader.Index - 1] == '.')
            {
                throw new ParserException("Illegal character found: .");
            }

            var literal = new string(reader.Source, ixold, reader.Index - ixold);
            return new XTokenNumber(double.Parse(literal));
        }


        private static XToken ScanString(
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

            return new XTokenString(literal);
        }


        private static XToken ScanLiteral(
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
                case "false": return new XTokenBoolean(false);
                case "true": return new XTokenBoolean(true);
                case "null": return new XTokenNull();
                case "match": return new XTokenOperMatch();
                default: return new XTokenRefId(literal);
            }
        }


        private static XToken ScanRegex(
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

            var token = new XTokenMatchParam(literal);
            token.Flags = flags;
            return token;
        }


        private static XToken ScanTwins(
            Reader reader,
            char head,
            XToken result
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) == false || ch != head)
            {
                throw new ParserException($"Illegal character found: {ch}");
            }

            reader.MoveNext();
            return result;
        }


        private static XToken ScanExclamationSign(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) && ch == '=')
            {
                reader.MoveNext();
                return new XTokenOperNotEqual();
            }
            else
            {
                return new XTokenOperNot();
            }
        }


        private static XToken ScanEqualSign(
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
                    case '=': return new XTokenOperEqual();
                }
            }
            throw new ParserException($"Illegal character found: {ch}");
        }


        private static XToken ScanLessSign(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) && ch == '=')
            {
                reader.MoveNext();
                return new XTokenOperLessOrEqualThan();
            }
            else
            {
                return new XTokenOperLessThan();
            }
        }


        private static XToken ScanGreaterSign(
            Reader reader,
            char head
            )
        {
            reader.MoveNext();

            char ch;
            if (reader.TryPeek(out ch) && ch == '=')
            {
                reader.MoveNext();
                return new XTokenOperGreaterOrEqualThan();
            }
            else
            {
                return new XTokenOperGreaterThan();
            }
        }

    }
}
