using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public interface IXSolverContext
    {
        IXReferenceSolver ReferenceSolver { get; }
    }

    public interface IXReferenceSolver
    {
        XSolverResult GetValue(XTokenRefId token);
    }

    public abstract class XReferenceSolverBase<T> : IXReferenceSolver
    {
        public T Context { get; set; }

        public abstract XSolverResult GetValue(XTokenRefId token);
    }

    public sealed class XSolverResult
    {
        private XSolverResult() { }

        public static XSolverResult FromData(object data)
        {
            return new XSolverResult { Data = data };
        }

        public static XSolverResult FromError(Exception error)
        {
            return new XSolverResult { Error = error };
        }

        public object Data { get; private set; }
        public Exception Error { get; private set; }
    }
}
