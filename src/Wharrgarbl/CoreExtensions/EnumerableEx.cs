namespace Wharrgarbl.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Various enumerable extension methods to make working with enumerable easier.
    /// </summary>
    public static class EnumerableEx
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> values, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            Contract.EndContractBlock();

            return values.Where(predicate.AsFunc());
        }

        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> values, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            Contract.EndContractBlock();

            return values.Where(predicate.Negated());
        }

        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> values, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            Contract.EndContractBlock();

            return values.Where(predicate.NegatedFunc());
        }

        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            Contract.EndContractBlock();

            foreach (var value in values)
            {
                action(value);
            }
        }
    }
}
