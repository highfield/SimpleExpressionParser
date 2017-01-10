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

    public sealed class SolverResult
    {
        private SolverResult() { }

        public static SolverResult FromData(object data)
        {
            return new SolverResult { Data = data };
        }

        public static SolverResult FromError(Exception error)
        {
            return new SolverResult { Error = error };
        }

        public object Data { get; private set; }
        public Exception Error { get; private set; }
    }
}
