namespace Wharrgarbl.Tests.Lifetimes
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

    public static class EnvVarLifetimeFacts
    {
        [Fact]
        public static void can_set_and_restore_process_var()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            {
                Environment.SetEnvironmentVariable("TEST_VAR", "old_value", EnvironmentVariableTarget.Process);

                using (var lifetime = EnvVarLifetime.Set("TEST_VAR", "new_value", EnvironmentVariableTarget.Process))
                {
                    Environment.GetEnvironmentVariable("TEST_VAR", EnvironmentVariableTarget.Process)
                    .Should().Be("new_value");
                }

                Environment.GetEnvironmentVariable("TEST_VAR", EnvironmentVariableTarget.Process)
                    .Should().Be("old_value");
            }
        }

        [Fact]
        public static void finalizer_restores_environment_variable()
        {
            lock (ATerribleMistake.EnvironmentVariableLock)
            using (var outerLifetime = EnvVarLifetime.Set("TEST_VAR", "outer"))
            {
                Fn.act(() =>
                {
                    EnvVarLifetime.Set("TEST_VAR", "inner");
                    Environment.GetEnvironmentVariable("TEST_VAR").Should().Be("inner");
                })(); // wrapping it inside an action and invoking should allow collection

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Environment.GetEnvironmentVariable("TEST_VAR").Should().Be("outer");
            }
        }
    }
}
