using Apicalypse.DotNet.Interpreters;
using Apicalypse.DotNet.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Tests.Interpreters
{
    public class SelectTypeInterpreter_RunShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReturnName()
        {
            var expected = "name";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameName>());
        }

        [Test]
        public void ReturnMultiple()
        {
            var expected = "name,slug,follows,alternative_names";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameShort>());
        }
    }
}
