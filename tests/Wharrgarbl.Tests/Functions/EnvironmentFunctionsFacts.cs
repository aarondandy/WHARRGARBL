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
    using Wharrgarbl.Tests.TestHelpers;
    using Xunit;

    public static class EnvironmentFunctionsFacts
    {
        [Fact]
        public static void can_get_variable()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            using (EnvVarLifetime.Set("TEST_VAR", "abc123"))
            {
                var result = EnvFn.GetEnvVar("TEST_VAR");

                result.Should().Be("abc123");
            }
        }

        [Fact]
        public static void can_set_variable()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            using (EnvVarLifetime.Set("TEST_VAR", "abc123"))
            {
                EnvFn.SetEnvVar("TEST_VAR", "def456");

                var result = EnvFn.GetEnvVar("TEST_VAR");

                result.Should().Be("def456");
            }
        }

        [Fact]
        public static void get_does_not_return_empty_string()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            using (EnvVarLifetime.Set("TEST_VAR", string.Empty))
            {
                var result = EnvFn.GetEnvVar("TEST_VAR");

                result.Should().Be(null);
            }
        }

        [Fact]
        public static void set_does_not_return_empty_string()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            using (EnvVarLifetime.Set("TEST_VAR", "junk"))
            {
                var result = EnvFn.SetEnvVar("TEST_VAR", string.Empty);

                result.Should().Be(null);
            }
        }

        [Fact]
        public static void delete_removes_value()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            using (EnvVarLifetime.Set("TEST_VAR", "junk"))
            {
                EnvFn.DeleteEnvVar("TEST_VAR");

                EnvFn.GetEnvVar("TEST_VAR").Should().BeNull();
            }
        }

        [Fact]
        public static void delete_preserves_old_value()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            using (EnvVarLifetime.Set("TEST_VAR", "deleted"))
            {
                var oldValue = EnvFn.DeleteEnvVar("TEST_VAR");

                oldValue.Should().Be("deleted");
            }
        }
    }
}
