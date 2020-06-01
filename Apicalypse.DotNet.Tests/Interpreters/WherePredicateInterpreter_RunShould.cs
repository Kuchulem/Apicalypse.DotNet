using Apicalypse.DotNet.Interpreters;
using Apicalypse.DotNet.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Apicalypse.DotNet.Extensions;

namespace Apicalypse.DotNet.Tests.Interpreters
{
    class WherePredicateInterpreter_RunShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReturnStringEqualToTest()
        {
            var expected = "name = \"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name == "Foo";

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringDifferentFromTest()
        {
            var expected = "name != \"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name != "Foo";

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnSimpleIntTest()
        {
            var expected = "follows = 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows == 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringContainsTest()
        {
            var expected = "name = *\"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.Contains("Foo");

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringContainsIgnoreCaseTest()
        {
            var expected = "name ~ *\"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.Contains("Foo", StringComparison.InvariantCultureIgnoreCase);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringStartsWithTest()
        {
            var expected = "name = *\"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.StartsWith("Foo");

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringStartsWithIgnoreCaseTest()
        {
            var expected = "name ~ *\"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.StartsWith("Foo", StringComparison.InvariantCultureIgnoreCase);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringStartsWithIgnoreCaseVariantTest()
        {
            var expected = "name ~ *\"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.StartsWith("Foo", true, CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringEndsWithTest()
        {
            var expected = "name = \"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.EndsWith("Foo");

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringEndsWithIgnoreCaseTest()
        {
            var expected = "name ~ \"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.EndsWith("Foo", StringComparison.InvariantCultureIgnoreCase);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringEndsWithIgnoreCaseVariantTest()
        {
            var expected = "name ~ \"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.EndsWith("Foo", true, CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntEqualToTest()
        {
            var expected = "follows = 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows == 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntDifferentFromTest()
        {
            var expected = "follows != 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows != 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntGreaterThanTest()
        {
            var expected = "follows > 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows > 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntGreaterThanEqualTest()
        {
            var expected = "follows >= 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows >= 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntLowerThanTest()
        {
            var expected = "follows < 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows < 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntLowerThanEqualTest()
        {
            var expected = "follows <= 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows <= 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringInNewArrayTest()
        {
            var expected = "name = (\"Foo\",\"Bar\",\"Baz\")";

            Expression<Func<Game, bool>> predicate = (g) => new[] { "Foo", "Bar", "Baz" }.Contains(g.Name);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringNotInNewArrayTest()
        {
            var expected = "name = !(\"Foo\",\"Bar\",\"Baz\")";

            Expression<Func<Game, bool>> predicate = (g) => !(new[] { "Foo", "Bar", "Baz" }.Contains(g.Name));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntInNewArrayTest()
        {
            var expected = "follows = (3,4,5)";

            Expression<Func<Game, bool>> predicate = (g) => new uint[] { 3,4,5 }.Contains(g.Follows);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntNotInNewArrayTest()
        {
            var expected = "follows = !(3,4,5)";

            Expression<Func<Game, bool>> predicate = (g) => !(new uint[] { 3, 4, 5 }.Contains(g.Follows));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntMatchesNewArrayTest()
        {
            var expected = "alternative_names = [1,2,3]";

            Expression<Func<Game, bool>> predicate = (g) => new int[] { 1,2,3 }.IsContainedIn(g.AlternativeNames);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringMatchesNotNewArrayTest()
        {
            var expected = "alternative_names = ![1,2,3]";

            Expression<Func<Game, bool>> predicate = (g) => !(new int[] { 1, 2, 3 }.IsContainedIn(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnIntMatchesAllNewArrayTest()
        {
            var expected = "alternative_names = {1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) => new int[] { 1, 2, 3 }.Equals(g.AlternativeNames);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnStringMatchesAllNotNewArrayTest()
        {
            var expected = "alternative_names = !{1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) => !(new int[] { 1, 2, 3 }.Equals(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnMultipleFiltersTest()
        {
            var expected = "name = \"Foo\"* & follows = (3,4,5) & alternative_names = !{1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) => 
                g.Name.EndsWith("Foo")
                && new uint[] { 3, 4, 5 }.Contains(g.Follows)
                && !(new int[] { 1, 2, 3 }.Equals(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnMultipleFiltersWithOrTest()
        {
            var expected = "name = \"Foo\"* & follows = (3,4,5) | alternative_names = !{1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) =>
                g.Name.EndsWith("Foo")
                && new uint[] { 3, 4, 5 }.Contains(g.Follows)
                || !(new int[] { 1, 2, 3 }.Equals(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body));
        }
    }
}
