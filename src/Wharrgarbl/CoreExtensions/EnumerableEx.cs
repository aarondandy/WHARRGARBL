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
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> values, Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return values.Where(predicate.Negated());
        }

        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (action == null) throw new ArgumentNullException("action");
            Contract.EndContractBlock();

            foreach (var value in values)
            {
                action(value);
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> values, T value)
        {
            if (values == null) throw new ArgumentNullException("values");
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return SingleConcatIterator(values, value);
        }

        public static IEnumerable<T> ConcatIf<T>(this IEnumerable<T> firstSet, IEnumerable<T> secondSet, bool condition)
        {
            if (firstSet == null) throw new ArgumentNullException("firstSet");
            if (condition && secondSet == null) throw new ArgumentException("secondSet must not be null if condition is true", "secondSet");
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return condition
                ? firstSet.Concat(secondSet)
                : firstSet;
        }

        public static IEnumerable<T> ConcatIf<T>(this IEnumerable<T> values, T value, bool condition)
        {
            if (values == null) throw new ArgumentNullException("firstSet");
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return condition
                ? values.Concat(value)
                : values;
        }

        private static IEnumerable<T> SingleConcatIterator<T>(IEnumerable<T> values, T appendValue)
        {
            foreach (var value in values) yield return value;
            yield return appendValue;
        }
    }
}
