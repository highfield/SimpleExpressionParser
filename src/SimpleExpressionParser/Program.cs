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
        static IXSolverContext _ctx = new MySolverContext();

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
            Test("1.6e-19");
            Test(" 6.022E23 ");
            Test(" 5E-1 ");
            Test(" .5 ");   //should throw
            Test(" -.5 ");   //should throw
            Test(" +5. ");   //should throw
            Test("myvar  ");
            Test("  w0rd");
            Test("_abc_def_");
            Test("my.multi.level.reference");
            Test("'single-quoted string'");
            Test("\"double-quoted string\"");
            //Test("'here is a \"nested string\"'");
            //Test(@"'here is a ""nested escaped string""'");

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
            Test("pname match /xyz/i");
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
                XTreeNodeBase node = XTreeNodeBase.Parse("! (a && (b && c || d && e) || (g == h && j))");
                const int N = 1000;

                var sw = new Stopwatch();
                sw.Start();

                XSolverResult sr;
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
            Console.WriteLine($"Source expression: {text}");
            try
            {
                //parsing
                XTreeNodeBase xtree = XTreeNodeBase.Parse(text);

                //compact serialization
                var cser = new XTreeCompactSerializer();
                cser.ShouldPad = true;
                string xstr = cser.Serialize(xtree);
                Console.WriteLine($"Serialized: {xstr}");

                //xml serialization
                var xser = new XTreeXmlSerializer();
                XElement xelem = xser.Serialize(xtree);
                Console.WriteLine(xelem);

                //evaluation (against the sample context)
                XSolverResult sr = xtree.Resolve(_ctx);
                Console.WriteLine($"Result: {sr.Error ?? sr.Data}");

                //verify the compact serialization
                try
                {
                    XTreeNodeBase xtreeAlt = XTreeNodeBase.Parse(xstr);
                    XElement xelemAlt = xser.Serialize(xtreeAlt);
                    if (xelem.ToString() != xelemAlt.ToString())
                    {
                        //fail!
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Verify error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test error: " + ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine();
#endif
        }

    }
}
