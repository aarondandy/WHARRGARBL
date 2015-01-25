namespace Wharrgarbl.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Various functions for the system environment designed for the using static feature.
    /// </summary>
    public static class EnvFn
    {
        public static string GetEnvVar(string variable)
        {
            Contract.Requires(!string.IsNullOrEmpty(variable));
            Contract.Ensures(Contract.Result<string>() != string.Empty);
            return Environment.GetEnvironmentVariable(variable);
        }

        public static string SetEnvVar(string variable, string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(variable));
            Contract.Ensures(Contract.Result<string>() != string.Empty);
            Environment.SetEnvironmentVariable(variable, value);
            return value == string.Empty ? null : value;
        }

        public static string DeleteEnvVar(string variable)
        {
            Contract.Requires(!string.IsNullOrEmpty(variable));
            Contract.Ensures(Contract.Result<string>() != string.Empty);
            var oldValue = GetEnvVar(variable);
            SetEnvVar(variable, null);
            return oldValue;
        }
    }
}
