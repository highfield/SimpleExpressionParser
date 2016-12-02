using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public interface ISolverContext
    {
        IReferenceSolver ReferenceSolver { get; }
    }

    public interface IReferenceSolver
    {
        SolverResult GetValue(XTokenRefId token);
    }

    public abstract class ReferenceSolverBase<T> : IReferenceSolver
    {
        public T Context { get; set; }

        public abstract SolverResult GetValue(XTokenRefId token);
    }

    public class SolverResult
    {
        public object Data { get; set; }
        public Exception Error { get; set; }
    }
}
