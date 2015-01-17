namespace Wharrgarbl.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class EnvFn
    {
        public static string GetEnvVar(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        public static string SetEnvVar(string variable, string value)
        {
            Environment.SetEnvironmentVariable(variable, value);
            return value;
        }
    }
}
