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
using Apicalypse.DotNet.Configuration;

namespace Apicalypse.DotNet.Tests.Interpreters
{
    class WherePredicateInterpreter_RunShould
    {
        RequestBuilderConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            configuration = new RequestBuilderConfiguration { CaseContract = CaseContract.SnakeCase };
        }

        [Test]
        public void ReturnStringEqualToTest()
        {
            var expected = "name = \"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name == "Foo";

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringEqualCamelCaseToTest()
        {
            var expected = "name = \"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name == "Foo";

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, new RequestBuilderConfiguration { CaseContract = CaseContract.CamelCase }));
        }

        [Test]
        public void ReturnStringEqualPascalCaseToTest()
        {
            var expected = "Name = \"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name == "Foo";

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, new RequestBuilderConfiguration { CaseContract = CaseContract.PascalCase }));
        }

        [Test]
        public void ReturnStringDifferentFromTest()
        {
            var expected = "name != \"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name != "Foo";

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnSimpleIntTest()
        {
            var expected = "follows = 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows == 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringContainsTest()
        {
            var expected = "name = *\"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.Contains("Foo");

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringContainsIgnoreCaseTest()
        {
            var expected = "name ~ *\"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.Contains("Foo", StringComparison.InvariantCultureIgnoreCase);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringStartsWithTest()
        {
            var expected = "name = \"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.StartsWith("Foo");

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringStartsWithIgnoreCaseTest()
        {
            var expected = "name ~ \"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.StartsWith("Foo", StringComparison.InvariantCultureIgnoreCase);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringStartsWithIgnoreCaseVariantTest()
        {
            var expected = "name ~ \"Foo\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.StartsWith("Foo", true, CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringEndsWithTest()
        {
            var expected = "name = *\"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.EndsWith("Foo");

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringEndsWithIgnoreCaseTest()
        {
            var expected = "name ~ *\"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.EndsWith("Foo", StringComparison.InvariantCultureIgnoreCase);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringEndsWithIgnoreCaseVariantTest()
        {
            var expected = "name ~ *\"Foo\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Name.EndsWith("Foo", true, CultureInfo.InvariantCulture);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntEqualToTest()
        {
            var expected = "follows = 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows == 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntDifferentFromTest()
        {
            var expected = "follows != 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows != 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntGreaterThanTest()
        {
            var expected = "follows > 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows > 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntGreaterThanEqualTest()
        {
            var expected = "follows >= 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows >= 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntLowerThanTest()
        {
            var expected = "follows < 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows < 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntLowerThanEqualTest()
        {
            var expected = "follows <= 3";

            Expression<Func<Game, bool>> predicate = (g) => g.Follows <= 3;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringInNewArrayTest()
        {
            var expected = "name = (\"Foo\",\"Bar\",\"Baz\")";

            Expression<Func<Game, bool>> predicate = (g) => new[] { "Foo", "Bar", "Baz" }.Contains(g.Name);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringNotInNewArrayTest()
        {
            var expected = "name = !(\"Foo\",\"Bar\",\"Baz\")";

            Expression<Func<Game, bool>> predicate = (g) => !(new[] { "Foo", "Bar", "Baz" }.Contains(g.Name));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntInNewArrayTest()
        {
            var expected = "follows = (3,4,5)";

            Expression<Func<Game, bool>> predicate = (g) => new uint[] { 3,4,5 }.Contains(g.Follows);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntNotInNewArrayTest()
        {
            var expected = "follows = !(3,4,5)";

            Expression<Func<Game, bool>> predicate = (g) => !(new uint[] { 3, 4, 5 }.Contains(g.Follows));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntMatchesNewArrayTest()
        {
            var expected = "alternative_names = [1,2,3]";

            Expression<Func<Game, bool>> predicate = (g) => new int[] { 1,2,3 }.IsContainedIn(g.AlternativeNames);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringMatchesNotNewArrayTest()
        {
            var expected = "alternative_names = ![1,2,3]";

            Expression<Func<Game, bool>> predicate = (g) => !(new int[] { 1, 2, 3 }.IsContainedIn(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnIntMatchesAllNewArrayTest()
        {
            var expected = "alternative_names = {1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) => new int[] { 1, 2, 3 }.Equals(g.AlternativeNames);

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnStringMatchesAllNotNewArrayTest()
        {
            var expected = "alternative_names = !{1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) => !(new int[] { 1, 2, 3 }.Equals(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnMultipleFiltersTest()
        {
            var expected = "name = *\"Foo\" & follows = (3,4,5) & alternative_names = !{1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) => 
                g.Name.EndsWith("Foo")
                && new uint[] { 3, 4, 5 }.Contains(g.Follows)
                && !(new int[] { 1, 2, 3 }.Equals(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnMultipleFiltersWithOrTest()
        {
            var expected = "name = *\"Foo\" & follows = (3,4,5) | alternative_names = !{1,2,3}";

            Expression<Func<Game, bool>> predicate = (g) =>
                g.Name.EndsWith("Foo")
                && new uint[] { 3, 4, 5 }.Contains(g.Follows)
                || !(new int[] { 1, 2, 3 }.Equals(g.AlternativeNames));

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnFullOneLevelPathFiltersWithOrTest()
        {
            var expected = "franchise.name = \"Worms\"";

            Expression<Func<Game, bool>> predicate = (g) => g.Franchise.Name == "Worms";

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnFullTwoLevelPathFiltersWithOrTest()
        {
            var expected = "cover.picture.url = \"https://covers.com\"*";

            Expression<Func<Game, bool>> predicate = (g) => g.Cover.Picture.Url.StartsWith("https://covers.com");

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnBooleanComparisonTest()
        {
            var expected = "early_access = true";

            Expression<Func<Game, bool>> predicate = (g) => g.EarlyAccess == true; ;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnBooleanFalseComparisonTest()
        {
            var expected = "early_access = false";

            Expression<Func<Game, bool>> predicate = (g) => g.EarlyAccess == false; ;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnNullComparisonTest()
        {
            var expected = "slug = null";

            Expression<Func<Game, bool>> predicate = (g) => g.Slug == null; ;

            Assert.AreEqual(expected, WherePredicateInterpreter.Run(predicate.Body, configuration));
        }
    }
}
