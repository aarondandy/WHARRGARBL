namespace Wharrgarbl.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Various string extension methods to make working with strings easier and more fluent.
    /// </summary>
    public static class StringEx
    {
        public static string Join(this string[] values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string Join(this object[] values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string Join<T>(this IEnumerable<T> values, string separator)
        {
            return string.Join<T>(separator, values);
        }

        public static IEnumerable<string> WhereHasText(this IEnumerable<string> values)
        {
            return values.Where(value => !String.IsNullOrEmpty(value)); // TODO: replace with WhereNot
        }

    }
}
