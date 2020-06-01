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
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ReturnPropertyNameTest()
        {
            var expected = "name";

            Expression<Func<GameShort, string>> predicate = g => g.Name;

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body));
        }

        [Test]
        public void ReturnMultiplePropertiesTest()
        {
            var expected = "name,follows";

            Expression<Func<GameShort, object>> predicate = g => new { g.Name, g.Follows };

            Assert.AreEqual(expected, MemberPredicateInterpreter.Run(predicate.Body));
        }
    }
}
