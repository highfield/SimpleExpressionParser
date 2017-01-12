using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cet.Core.Expression
{
    public static class ExpressionExtensions
    {

        public static XTreeNodeBase Associate<TOper>(
            this IEnumerable<XTreeNodeBase> source
            )
            where TOper : XToken, ITokenAssociative, new()
        {
            XTreeNodeBase result = null;
            var iter = source.GetEnumerator();
            while (iter.MoveNext())
            {
                if (result == null)
                {
                    result = iter.Current;
                }
                else
                {
                    result = new XTreeNodeBinary(
                        new TOper(),
                        result,
                        iter.Current
                        );
                }
            }
            return result;
        }


        public static Func<TIn, object> GetLambda<TIn>(
            this XTreeNodeBase resolver,
            XReferenceSolverBase<TIn> refres
            )
        {
            if (resolver == null) return null;

            var rc = new MyResolverContext() { ReferenceSolver = refres };

            Func<TIn, object> func = input =>
            {
                refres.Context = input;
                XSolverResult result = resolver.Resolve(rc);
                if (result.Error == null)
                {
                    return result.Data;
                }
                else
                {
                    throw result.Error;
                }
            };

            return func;
        }


        public static Func<TIn, TOut> GetLambda<TIn, TOut>(
            this XTreeNodeBase resolver,
            XReferenceSolverBase<TIn> refres
            )
        {
            if (resolver == null) return null;

            var rc = new MyResolverContext() { ReferenceSolver = refres };

            Func<TIn, TOut> func = input =>
            {
                refres.Context = input;
                XSolverResult result = resolver.Resolve(rc);
                if (result.Error == null)
                {
                    return (TOut)result.Data;
                }
                else
                {
                    throw result.Error;
                }
            };

            return func;
        }


        private class MyResolverContext : IXSolverContext
        {
            public IXReferenceSolver ReferenceSolver { get; set; }
        }

    }
}
