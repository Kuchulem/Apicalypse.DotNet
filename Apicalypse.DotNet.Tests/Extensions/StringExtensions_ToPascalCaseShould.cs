using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Apicalypse.DotNet.Extensions;

namespace Apicalypse.DotNet.Tests.Extensions
{
    class StringExtensions_ToPascalCaseShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertCamelCaseToPascalCase()
        {
            var original = "someCamelCaseString";
            var expected = "SomeCamelCaseString";
            
            Assert.AreEqual(expected, original.ToPascalCase());
        }

        [Test]
        public void ConvertPascalCaseToPascalCase()
        {
            var original = "some_snake_case_string";
            var expected = "SomeSnakeCaseString";

            Assert.AreEqual(expected, original.ToPascalCase());
        }
    }
}
