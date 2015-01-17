﻿namespace Wharrgarbl.Tests.Lifetimes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Wharrgarbl.Lifetimes;
    using Xunit;

    public static class EnvVarLifetimeFacts
    {
        [Fact]
        public static void can_set_and_restore_process_var()
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

        [Fact]
        public static void finalizer_restores_environment_variable()
        {
            using (var outerLifetime = EnvVarLifetime.Set("TEST_VAR", "outer"))
            {
                Action innerLifetime = () =>
                {
                    EnvVarLifetime.Set("TEST_VAR", "inner");
                    Environment.GetEnvironmentVariable("TEST_VAR").Should().Be("inner");
                };
                innerLifetime(); // wrapping it inside an action and invoking should allow collection

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Environment.GetEnvironmentVariable("TEST_VAR").Should().Be("outer");
            }
        }
    }
}