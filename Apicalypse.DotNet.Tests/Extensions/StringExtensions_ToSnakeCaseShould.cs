using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Apicalypse.DotNet.Extensions;

namespace Apicalypse.DotNet.Tests.Extensions
{
    class StringExtensions_ToSnakeCaseShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertCamelCaseToSnakeCase()
        {
            var original = "someCamelCaseString";
            var expected = "some_camel_case_string";

            Assert.AreEqual(expected, original.ToSnakeCase());
        }

        [Test]
        public void ConvertPascalCaseToSnakeCase()
        {
            var original = "SomePascalCaseString";
            var expected = "some_pascal_case_string";

            Assert.AreEqual(expected, original.ToSnakeCase());
        }
    }
}
