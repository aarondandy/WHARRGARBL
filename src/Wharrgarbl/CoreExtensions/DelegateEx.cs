namespace Wharrgarbl.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods dealing with delegates.
    /// </summary>
    public static class DelegateEx
    {
        /// <summary>
        /// Casts a predicate to the matching Func type.
        /// </summary>
        /// <typeparam name="T">The input type for the predicate.</typeparam>
        /// <param name="predicate">The predicate to cast.</param>
        /// <returns>A func of the given predicate.</returns>
        public static Func<T, bool> AsFunc<T>(this Predicate<T> predicate)
        {
            return new Func<T, bool>(predicate);
        }

        /// <summary>
        /// Casts a func to a predicate type.
        /// </summary>
        /// <typeparam name="T">The input type for the predicate.</typeparam>
        /// <param name="predicate">The func to cast.</param>
        /// <returns>A predicate of the given func.</returns>
        public static Predicate<T> AsPredicate<T>(this Func<T, bool> predicate)
        {
            return new Predicate<T>(predicate);
        }

        /// <summary>
        /// Creates a logically negated version of a predicate.
        /// </summary>
        /// <typeparam name="T">The input type for the predicate.</typeparam>
        /// <param name="predicate">The original predicate.</param>
        /// <returns>A logically negated predicate of the given predicate.</returns>
        public static Func<T, bool> Negated<T>(this Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Contract.Ensures(Contract.Result<Func<T, bool>>() != null);
            return x => !predicate(x);
        }

        /// <summary>
        /// Creates a logically negated version of a predicate.
        /// </summary>
        /// <typeparam name="T">The input type for the predicate.</typeparam>
        /// <param name="predicate">The original predicate.</param>
        /// <returns>A logically negated predicate of the given predicate.</returns>
        public static Predicate<T> Negated<T>(this Predicate<T> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Contract.Ensures(Contract.Result<Predicate<T>>() != null);
            return x => !predicate(x);
        }

        /// <summary>
        /// Creates a logically negated version of a predicate that also casts to a func.
        /// </summary>
        /// <typeparam name="T">The input type for the predicate.</typeparam>
        /// <param name="predicate">The original predicate.</param>
        /// <returns>A logically negated func of the given predicate.</returns>
        public static Func<T, bool> NegatedFunc<T>(this Predicate<T> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Contract.Ensures(Contract.Result<Func<T, bool>>() != null);
            return x => !predicate(x);
        }
    }
}
