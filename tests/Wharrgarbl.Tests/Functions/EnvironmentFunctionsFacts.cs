namespace Wharrgarbl.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Wharrgarbl.Functions;
    using Wharrgarbl.Lifetimes;
    using Xunit;

    public static class EnvironmentFunctionsFacts
    {
        [Fact]
        public static void can_get_variable()
        {
            using (EnvVarLifetime.Set("TEST_VAR", "abc123"))
            {
                var result = EnvFn.GetEnvVar("TEST_VAR");

                result.Should().Be("abc123");
            }
        }

        [Fact]
        public static void can_set_variable()
        {
            using (EnvVarLifetime.Set("TEST_VAR", "abc123"))
            {
                EnvFn.SetEnvVar("TEST_VAR", "def456");

                var result = EnvFn.GetEnvVar("TEST_VAR");

                result.Should().Be("def456");
            }
        }
    }
}
