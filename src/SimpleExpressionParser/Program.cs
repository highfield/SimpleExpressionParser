//#define DOC

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cet.Core.Expression
{
    public class Program
    {
        static StringBuilder _sb = new StringBuilder();
        static ISolverContext _ctx = new MySolverContext();

        public static void Main(string[] args)
        {
            Test("");
            Test("    ");
            Test("false  ");
            Test("  true");
            Test("  null  ");
            Test(" 123 ");
            Test("-456  ");
            Test(" +7 ");
            Test(" 3.151927 ");
            Test(" +2.718 ");
            Test(" .5 ");   //should throw
            Test(" -.5 ");   //should throw
            Test(" +5. ");   //should throw
            Test("myvar  ");
            Test("  w0rd");
            Test("_abc_def_");
            Test("'single-quoted string'");
            Test("\"double-quoted string\"");
            Test("'here is a \"nested string\"'");

            Test("zero == zero  ");
            Test(" black != white");
            Test(" 12 < 45");
            Test("20 >4");
            Test("10<=100");
            Test("100   >=   1");

            Test("!false==!!true");
            Test("to_be || !to_be");
            Test(" maccheroni || spaghetti || rigatoni");
            Test(" sex && drug && rock && roll   ");
            Test("!me || you && !they ");
            Test("a==b && c!=d");
            Test("pname match/abc/");
            Test("pname match /xyz/ig");
            Test("pname   match /(\\w+)\\s(\\w+)/");

            Test("(!me ||you)&&they");
            Test("!(a=='q') && (b!='x')");
            Test("(a || b) && (c || d) || (e && f)");
            Test("! (a && (b && c || d && e) || (g == h && j))");
            Test("!! (((a)==b) && ((((c && ((g)))))))");

#if DOC
            System.IO.File.WriteAllText(@"i:\temp\parser_output.txt", _sb.ToString());
#endif

            {
                TreeNodeBase node = Parser.Parse("! (a && (b && c || d && e) || (g == h && j))");
                const int N = 1000;

                var sw = new Stopwatch();
                sw.Start();

                SolverResult sr;
                for (int i = 0; i < N; i++)
                {
                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);

                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);
                    sr = node.Resolve(_ctx);
                }

                sw.Stop();
                Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms; {sw.ElapsedMilliseconds * 100.0 / N}us/each");
            }

            Console.WriteLine("Process complete.");
            Console.ReadKey();
        }


        private static void Test(
            string text
            )
        {
#if DOC
            _sb.AppendLine("Expression:");
            _sb.AppendFormat("`{0}`", text);
            _sb.AppendLine();
            _sb.AppendLine();
            _sb.AppendLine("Result:");
            _sb.AppendLine("```");
            try
            {
                TreeNodeBase node = Parser.Parse(text);
                XElement xelem = ToXml(node);
                _sb.Append(xelem);
            }
            catch (Exception ex)
            {
                _sb.Append(ex.Message);
            }
            _sb.AppendLine();
            _sb.AppendLine("```");
            _sb.AppendLine("***");
            _sb.AppendLine();
            _sb.AppendLine();
#else
            Console.WriteLine($"Expression: {text}");
            try
            {
                TreeNodeBase node = Parser.Parse(text);
                XElement xelem = ToXml(node);
                Console.WriteLine(xelem);

                SolverResult sr = node.Resolve(_ctx);
                Console.WriteLine($"Result: {sr.Error ?? sr.Data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine();
#endif
        }


        private static XElement ToXml(TreeNodeBase node)
        {
            var xelem = new XElement(
                node.Token.GetType().Name,
                node.GetChildren().Select(_ => ToXml(_))
                );

            if (node.Token.Data != null)
            {
                xelem.Add(new XAttribute("data", node.Token.Data));
            }

            var mp = node.Token as XTokenMatchParam;
            if (mp != null)
            {
                xelem.Add(new XAttribute("flags", mp.Flags));
            }
            return xelem;
        }

    }
}
