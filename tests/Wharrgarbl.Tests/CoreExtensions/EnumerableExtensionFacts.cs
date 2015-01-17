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

    public static class EnumerableExtensionFacts
    {
        [Fact]
        public static void where_not_and_where_results_are_disjoint()
        {
            var numbers = Enumerable.Range(0, 10);
            Predicate<int> isEven = x => x % 2 == 0;
            var evenNumbers = numbers.Where(isEven.AsFunc());

            var oddNumbers = numbers.WhereNot(isEven.AsFunc());

            oddNumbers.Should().BeEquivalentTo(new[] { 1, 3, 5, 7, 9 });
            evenNumbers.ForEach(n => oddNumbers.Should().NotContain(n));
        }

        [Fact]
        public static void where_for_predicate()
        {
            var numbers = Enumerable.Range(0, 5);
            Predicate<int> isEven = x => x % 2 == 0;

            var evenNumbers = numbers.Where(isEven);

            evenNumbers.Should().BeEquivalentTo(new[] { 0, 2, 4 });
        }

        [Fact]
        public static void where_pred_and_func_match()
        {
            var numbers = Enumerable.Range(0, 10);
            Predicate<int> isEven = x => x % 2 == 0;

            var predicateNumbers = numbers.Where(isEven);
            var funcNumbers = numbers.Where(isEven.AsFunc());

            predicateNumbers.Should().BeEquivalentTo(funcNumbers);
        }

        [Fact]
        public static void where_not_pred_and_func_match()
        {
            var numbers = Enumerable.Range(0, 10);
            Predicate<int> isEven = x => x % 2 == 0;

            var predicateNumbers = numbers.WhereNot(isEven);
            var funcNumbers = numbers.WhereNot(isEven.AsFunc());

            predicateNumbers.Should().BeEquivalentTo(funcNumbers);
        }

        [Fact]
        public static void single_append_adds_to_existing()
        {
            var data = new[] { 1, 2, 3 };

            var result = data.Concat(4);

            result.ShouldAllBeEquivalentTo(new[] { 1, 2, 3, 4 });
        }

        public static void single_append_adds_to_empty()
        {
            var result = Enumerable.Empty<int>().Concat(1);

            result.ShouldAllBeEquivalentTo(new[] { 1 });
        }
    }
}
