using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cet.Core.Expression
{
    public class Program
    {
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

            Console.WriteLine("Process complete.");
            Console.ReadKey();
        }


        private static void Test(
            string text
            )
        {
            Console.WriteLine($"Expression: {text}");
            try
            {
                TreeNodeBase node = Parser.Parse(text);
                XElement xelem = ToXml(node);
                Console.WriteLine(xelem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
        }


        private static XElement ToXml(TreeNodeBase node)
        {
            var xelem = new XElement(
                node.Token.Type.ToString(),
                node.GetChildren().Select(_ => ToXml(_))
                );

            if (node.Token.Data != null)
            {
                xelem.Add(new XAttribute("data", node.Token.Data));
            }

            if (node.Token.Param != null)
            {
                xelem.Add(new XAttribute("param", node.Token.Param));
            }
            return xelem;
        }

    }
}
