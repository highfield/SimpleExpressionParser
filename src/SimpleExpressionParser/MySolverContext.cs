using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    class MySolverContext : IXSolverContext
    {
        public IXReferenceSolver ReferenceSolver { get; } = new MyReferenceSolver();
    }

    class MyReferenceSolver : IXReferenceSolver
    {
        private Dictionary<string, object> _bag = new Dictionary<string, object>()
        {
            ["myvar"] = "MyVar",
            ["w0rd"] = null,
            ["_abc_def_"] = Math.PI,
            ["zero"] = 0,
            ["black"] = "nero",
            ["white"] = "bianco",
            ["to_be"] = true,
            ["maccheroni"] = "stringa",
            ["spaghetti"] = 1234,
            ["rigatoni"] = false,
            ["sex"] = 789,
            ["drug"] = "droga",
            ["rock"] = true,
            ["roll"] = 3.14159,
            ["me"] = null,
            ["you"] = 1,
            ["they"] = true,
            ["a"] = 555,
            ["b"] = 555,
            ["c"] = true,
            ["d"] = null,
            ["e"] = "abc",
            ["f"] = -34,
            ["g"] = "",
            ["h"] = null,
            ["j"] = false,
            ["pname"] = "very long text which contains 'abcdefgh'...",
        };

        public XSolverResult GetValue(XTokenRefId token)
        {
            var refId = token.Data as string;
            if (string.IsNullOrEmpty(refId)) return XSolverResult.FromData(null);

            object value;
            this._bag.TryGetValue(refId, out value);
            return XSolverResult.FromData(value);
        }
    }
}
