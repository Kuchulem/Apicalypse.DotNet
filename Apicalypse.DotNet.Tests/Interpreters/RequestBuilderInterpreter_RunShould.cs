using Apicalypse.DotNet;
using Apicalypse.DotNet.Interpreters;
using Apicalypse.DotNet.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apicalypse.DotNet.Tests.Interpreters
{
    class RequestBuilderInterpreter_RunShould
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ReturnSelectAllTest()
        {
            var expected = "fields *;";

            Assert.AreEqual(expected, RequestBuilderInterpreter.Run("*", "", "", "", "", 0, 0));
        }

        [Test]
        public void ReturnCompleteQueryTest()
        {
            var expected = "fields name,slug,follows,alternative_names;\n" +
                "where follows >= 3 & follows < 10;\n" +
                "sort name desc;\n" +
                "search \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, RequestBuilderInterpreter.Run(
                "name,slug,follows,alternative_names",
                "follows >= 3 & follows < 10", "",
                "name desc",
                "\"Foo\"",
                8,
                2
            ));
        }
    }
}
