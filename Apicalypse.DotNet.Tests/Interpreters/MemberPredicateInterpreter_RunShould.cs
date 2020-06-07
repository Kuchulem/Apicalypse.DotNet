using Apicalypse.DotNet.Configuration;
using Apicalypse.DotNet.Interpreters;
using Apicalypse.DotNet.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Apicalypse.DotNet.Tests.Interpreters
{
    class MemberPredicateInterpreter_RunShould
    {
        RequestBuilderConfiguration snakeCaseConfiguration;
        RequestBuilderConfiguration camelCaseConfiguration;
        RequestBuilderConfiguration pascalCaseConfiguration;

        [SetUp]
        public void Setup()
        {
            snakeCaseConfiguration = new RequestBuilderConfiguration { CaseContract = CaseContract.SnakeCase };
            camelCaseConfiguration = new RequestBuilderConfiguration { CaseContract = CaseContract.CamelCase };
            pascalCaseConfiguration = new RequestBuilderConfiguration { CaseContract = CaseContract.PascalCase };
        }

        [Test]
        public void ReturnPropertyNameSnakeCaseTest()
        {
            var expected = "name";

            Expression<Func<GameShort, string>> predicate = g => g.Name;

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, snakeCaseConfiguration));
        }

        [Test]
        public void ReturnMultiplePropertiesSnakeCaseTest()
        {
            var expected = "name,follows";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name, g.Follows };

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, snakeCaseConfiguration));
        }

        [Test]
        public void ReturnCompleteOneLevelPathSnakeCaseTest()
        {
            var expected = "franchise.name";
            Expression<Func<Game, object>> predicate = g => g.Franchise.Name;
            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, snakeCaseConfiguration));
        }

        [Test]
        public void ReturnCompleteTwoLevelsPathSnakeCaseTest()
        {
            var expected = "cover.picture.url";
            Expression<Func<Game, object>> predicate = g => g.Cover.Picture.Url;
            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, snakeCaseConfiguration));
        }

        [Test]
        public void ReturnPropertyNameCamelCaseTest()
        {
            var expected = "name";

            Expression<Func<GameShort, string>> predicate = g => g.Name;

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, camelCaseConfiguration));
        }

        [Test]
        public void ReturnMultiplePropertiesCamelCaseTest()
        {
            var expected = "name,follows";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name, g.Follows };

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, camelCaseConfiguration));
        }

        [Test]
        public void ReturnCompleteOneLevelPathCamelCaseTest()
        {
            var expected = "franchise.name";
            Expression<Func<Game, object>> predicate = g => g.Franchise.Name;
            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, camelCaseConfiguration));
        }

        [Test]
        public void ReturnCompleteTwoLevelsPathCamelCaseTest()
        {
            var expected = "cover.picture.url";
            Expression<Func<Game, object>> predicate = g => g.Cover.Picture.Url;
            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, camelCaseConfiguration));
        }

        [Test]
        public void ReturnPropertyNamePascalCaseTest()
        {
            var expected = "Name";

            Expression<Func<GameShort, string>> predicate = g => g.Name;

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, pascalCaseConfiguration));
        }

        [Test]
        public void ReturnMultiplePropertiesPascalCaseTest()
        {
            var expected = "Name,Follows";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name, g.Follows };

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, pascalCaseConfiguration));
        }

        [Test]
        public void ReturnCompleteOneLevelPathPascalCaseTest()
        {
            var expected = "Franchise.Name";
            Expression<Func<Game, object>> predicate = g => g.Franchise.Name;
            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, pascalCaseConfiguration));
        }

        [Test]
        public void ReturnCompleteTwoLevelsPascalCaseTest()
        {
            var expected = "Cover.Picture.Url";
            Expression<Func<Game, object>> predicate = g => g.Cover.Picture.Url;
            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body, pascalCaseConfiguration));
        }
    }
}
