using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Apicalypse.DotNet.Extensions;

namespace Apicalypse.DotNet.Tests.Extensions
{
    class Extensions_ToCamelCaseShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertPascalCaseToCamelCase()
        {
            var original = "SomePascalCaseString";
            var expected = "somePascalCaseString";

            Assert.AreEqual(expected, original.ToCamelCase());
        }

        [Test]
        public void ConvertPascalCaseToPascalCase()
        {
            var original = "some_snake_case_string";
            var expected = "someSnakeCaseString";

            Assert.AreEqual(expected, original.ToCamelCase());
        }
    }
}
