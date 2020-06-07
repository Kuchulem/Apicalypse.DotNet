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
    class InvertedOrderByInterpreter_RunShould
    {
        RequestBuilderConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            configuration = new RequestBuilderConfiguration { CaseContract = CaseContract.SnakeCase };
        }

        [Test]
        public void ReturnDescendingPropertyTest()
        {
            var expected = "name desc";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name };

            Assert.AreEqual(expected, InvertedOrderByInterpreter.Run(predicate.Body, configuration));
        }

        [Test]
        public void ReturnMultipleDescendingPropertiesTest()
        {
            var expected = "name desc,follows desc";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name, g.Follows };

            Assert.AreEqual(expected, InvertedOrderByInterpreter.Run(predicate.Body, configuration));
        }
    }
}
