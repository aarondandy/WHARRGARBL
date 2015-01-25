namespace Wharrgarbl.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Various functions for <see cref="System.Func{TResult}">Func</see>, <see cref="System.Action">Action</see> and friends designed for the using static feature.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Language hack for C# 6")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1611:ElementParametersMustBeDocumented", Justification = "I am too lazy for this.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1615:ElementReturnValueMustBeDocumented", Justification = "I am too lazy for this.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1618:GenericTypeParametersMustBeDocumented", Justification = "I am too lazy for this.")]
    public static class Fn
    {
        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<TResult> fun<TResult>(Func<TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T, TResult> fun<T, TResult>(Func<T, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T1, T2, TResult> fun<T1, T2, TResult>(Func<T1, T2, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T1, T2, T3, TResult> fun<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T1, T2, T3, T4, TResult> fun<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T1, T2, T3, T4, T5, TResult> fun<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T1, T2, T3, T4, T5, T6, TResult> fun<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> fun<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> fun<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> f)
        {
            return f;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action act(Action a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T> act<T>(Action<T> a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T1, T2> act<T1, T2>(Action<T1, T2> a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T1, T2, T3> act<T1, T2, T3>(Action<T1, T2, T3> a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T1, T2, T3, T4> act<T1, T2, T3, T4>(Action<T1, T2, T3, T4> a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T1, T2, T3, T4, T5> act<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T1, T2, T3, T4, T5, T6> act<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T1, T2, T3, T4, T5, T6, T7> act<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> a)
        {
            return a;
        }

        /// <summary>
        /// Wraps a delegate type to provide type information to the compiler.
        /// </summary>
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> act<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> a)
        {
            return a;
        }
    }
}
