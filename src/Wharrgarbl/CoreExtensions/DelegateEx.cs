namespace Wharrgarbl.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DelegateEx
    {
        public static Func<T, bool> AsFunc<T>(this Predicate<T> predicate)
        {
            return new Func<T, bool>(predicate);
        }

        public static Predicate<T> AsPredicate<T>(this Func<T, bool> predicate)
        {
            return new Predicate<T>(predicate);
        }

        public static Func<T, bool> Negated<T>(this Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Contract.Ensures(Contract.Result<Func<T, bool>>() != null);
            return x => !predicate(x);
        }

        public static Predicate<T> Negated<T>(this Predicate<T> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Contract.Ensures(Contract.Result<Predicate<T>>() != null);
            return x => !predicate(x);
        }

        public static Func<T, bool> NegatedFunc<T>(this Predicate<T> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Contract.Ensures(Contract.Result<Func<T, bool>>() != null);
            return x => !predicate(x);
        }
    }
}
