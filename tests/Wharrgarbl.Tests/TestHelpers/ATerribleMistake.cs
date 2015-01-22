namespace Wharrgarbl.Tests.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class ATerribleMistake
    {
        public static readonly object EnvironmentVariableLock = new object();
    }
}
