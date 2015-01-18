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

    public static class EnumFunctionsFacts
    {
        [Fact]
        public static void can_parse_enum()
        {
            var result = EnumFn.ParseEnum<DayOfWeek>("Tuesday");

            result.Should().Be(DayOfWeek.Tuesday);
        }

        [Fact]
        public static void can_parse_enum_case_insensitive()
        {
            var result = EnumFn.ParseEnum<DayOfWeek>("tuesday", true);

            result.Should().Be(DayOfWeek.Tuesday);
        }

        [Fact]
        public static void enum_is_not_found()
        {
            Fn.act(() => EnumFn.ParseEnum<DayOfWeek>("January"))
                .ShouldThrow<ArgumentException>();
        }

        [Fact]
        public static void enum_is_not_found_insensitive()
        {
            Fn.act(() => EnumFn.ParseEnum<DayOfWeek>("january", true))
                .ShouldThrow<ArgumentException>();
        }

        [Fact]
        public static void can_parse_nullable_enum()
        {
            var result = EnumFn.ParseEnumOrNull<DayOfWeek>("Tuesday");

            result.Should().Be(DayOfWeek.Tuesday);
        }

        [Fact]
        public static void can_parse_nullable_enum_case_insensitive()
        {
            var result = EnumFn.ParseEnumOrNull<DayOfWeek>("tuesday", true);

            result.Should().Be(DayOfWeek.Tuesday);
        }

        [Fact]
        public static void nullable_enum_is_null_when_not_found()
        {
            var result = EnumFn.ParseEnumOrNull<DayOfWeek>("January");

            result.Should().BeNull();
        }

        [Fact]
        public static void nullable_enum_is_null_when_not_found_insensitive()
        {
            var result = EnumFn.ParseEnumOrNull<DayOfWeek>("january", true);

            result.Should().BeNull();
        }
    }
}
