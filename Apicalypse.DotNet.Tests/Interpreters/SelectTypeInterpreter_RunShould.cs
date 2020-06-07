using Apicalypse.DotNet.Configuration;
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
        public void ReturnNameSnakeCaseTest()
        {
            var expected = "name";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameName>(new RequestBuilderConfiguration { CaseContract = CaseContract.SnakeCase }));
        }

        [Test]
        public void ReturnMultipleSnakeCaseTest()
        {
            var expected = "name,slug,follows,alternative_names,franchise.name";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameShort>(new RequestBuilderConfiguration { CaseContract = CaseContract.SnakeCase }));
        }

        [Test]
        public void ReturnNameCamelCaseTest()
        {
            var expected = "name";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameName>(new RequestBuilderConfiguration { CaseContract = CaseContract.CamelCase }));
        }

        [Test]
        public void ReturnMultipleCamelCaseTest()
        {
            var expected = "name,slug,follows,alternativeNames,franchise.name";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameShort>(new RequestBuilderConfiguration { CaseContract = CaseContract.CamelCase }));
        }

        [Test]
        public void ReturnNamePascalCaseTest()
        {
            var expected = "Name";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameName>(new RequestBuilderConfiguration { CaseContract = CaseContract.PascalCase }));
        }

        [Test]
        public void ReturnMultiplePascalCaseTest()
        {
            var expected = "Name,Slug,Follows,AlternativeNames,Franchise.Name";

            Assert.AreEqual(expected, SelectTypeInterpreter.Run<GameShort>(new RequestBuilderConfiguration { CaseContract = CaseContract.PascalCase }));
        }
    }
}
