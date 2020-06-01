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
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ReturnDescendingPropertyTest()
        {
            var expected = "name desc";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name };

            Assert.AreEqual(expected, InvertedOrderByInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnMultipleDescendingPropertiesTest()
        {
            var expected = "name desc,follows desc";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name, g.Follows };

            Assert.AreEqual(expected, InvertedOrderByInterpreter.Run(predicate.Body));
        }
    }
}
