namespace Wharrgarbl.Tests.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Wharrgarbl.CoreExtensions;
    using Xunit;

    public static class StringExtensionFacts
    {
        [Fact]
        public static void join_string_array()
        {
            var result = new[] { "one", "two", "three" }.Join("-");
            result.Should().Be("one-two-three");
        }

        [Fact]
        public static void join_object_array()
        {
            var result = new object[] { 1, 2, 3 }.Join("-");
            result.Should().Be("1-2-3");
        }

        [Fact]
        public static void join_string_enumerable()
        {
            var result = new[] { "one", "two", "three" }.AsEnumerable().Join("-");
            result.Should().Be("one-two-three");
        }

        [Fact]
        public static void join_object_enumerable()
        {
            var result = new object[] { 1, 2, 3 }.AsEnumerable().Join("-");
            result.Should().Be("1-2-3");
        }

        [Fact]
        public static void filter_out_null_and_empty()
        {
            var items = new[] { "1", null, "2", string.Empty, "3" };
            
            var result = items.WhereHasText();

            result.Should().BeEquivalentTo(new[] { "1", "2", "3" });
        }

        [Fact]
        public static void compare_case_insensitive_oridnal()
        {
            "LETTERS".EqualsOrdinal("letters", true).Should().BeTrue();
            "LETTERS".EqualsOrdinal("letters", false).Should().BeFalse();
            "LETTERS".EqualsOrdinal("numbers", true).Should().BeFalse();
            "LETTERS".EqualsOrdinal("numbers", false).Should().BeFalse();
        }
    }
}
