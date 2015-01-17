namespace Wharrgarbl.Tests.CoreExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Wharrgarbl.CoreExtensions;
    using Wharrgarbl.Functions;
    using Xunit;
  
    public static class DelegateExtensionFacts
    {
        [Fact]
        public static void pred_to_func_matches()
        {
            var numbers = Enumerable.Range(0, 10);
            var pred = new Predicate<int>(x => x % 2 == 0).AsFunc();
            var func = Fn.fun((int x) => x % 2 == 0);

            var predNumbers = numbers.Where(pred);
            var funcNumbers = numbers.Where(func);

            predNumbers.Should().BeEquivalentTo(funcNumbers);
        }

        [Fact]
        public static void func_to_pred_matches()
        {
            var numbers = Enumerable.Range(0, 10);
            var pred = new Predicate<int>(x => x % 2 == 0);
            var func = Fn.fun((int x) => x % 2 == 0).AsPredicate();

            var predNumbers = numbers.Where(pred);
            var funcNumbers = numbers.Where(func);

            predNumbers.Should().BeEquivalentTo(funcNumbers);
        }

        [Fact]
        public static void func_negated_check()
        {
            var numbers = Enumerable.Range(0, 6);
            var even = Fn.fun((int x) => x % 2 == 0);
            var odd = even.Negated();

            var oddNumbers = numbers.Where(odd);

            oddNumbers.ShouldAllBeEquivalentTo(new[] { 1, 3, 5 });
        }

        [Fact]
        public static void pred_negated_check()
        {
            var numbers = Enumerable.Range(0, 6);
            var even = new Predicate<int>(x => x % 2 == 0);
            var odd = even.Negated();

            var oddNumbers = numbers.Where(odd);

            oddNumbers.ShouldAllBeEquivalentTo(new[] { 1, 3, 5 });
        }

        [Fact]
        public static void pred_negated_as_func_check()
        {
            var numbers = Enumerable.Range(0, 6);
            var even = new Predicate<int>(x => x % 2 == 0);
            var odd = even.NegatedFunc();

            var oddNumbers = numbers.Where(odd);

            oddNumbers.ShouldAllBeEquivalentTo(new[] { 1, 3, 5 });
        }
    }
}
