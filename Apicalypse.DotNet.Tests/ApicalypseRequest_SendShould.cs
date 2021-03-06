﻿using Apicalypse.DotNet.Tests.Mocks;
using Apicalypse.DotNet.Tests.Models;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;

namespace Apicalypse.DotNet.Tests
{
    class ApicalypseRequest_SendShould
    {
        private HttpClient httpClient;

        [SetUp]
        public void Setup()
        {
            httpClient = new HttpClient(new HttpMessageHandlerMock())
            {
                BaseAddress = new System.Uri("http://apicalypse.local")
            };
        }

        [TearDown]
        public void TearDown()
        {
            httpClient.Dispose();
        }

        [Test]
        public void ReturnHttpResponseMessageTest()
        {
            var response = new RequestBuilder<Game>().Build().Send(httpClient, "game").GetAwaiter().GetResult();

            Assert.IsInstanceOf<HttpResponseMessage>(response);
        }

        [Test]
        public void SendsQueryTest()
        {
            var response = new RequestBuilder<Game>()
                .Select<GameShort>()
                .Where(g => g.Follows >= 3 && g.Follows < 10)
                .OrderByDescending(g =>g.Name)
                .Search("Foo")
                .Take(8)
                .Skip(2)
                .Build().Send<HttpMockModel>(httpClient, "game").GetAwaiter().GetResult();

            var expected = "fields name,slug,follows,alternative_names,franchise.name;\n" +
                "where follows >= 3 & follows < 10;\n" +
                "sort name desc;\n" +
                "search \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, response.First().RequestBody);
        }

        [Test]
        public void SendsQueryWithMultiLevelPathesTest()
        {
            var response = new RequestBuilder<Game>()
                .Select<GameShort>()
                .Where(g => g.Follows >= 3 && g.Follows < 10 && g.Cover.Picture.Url.StartsWith("https://gamecovers.com"))
                .OrderByDescending(g => g.Name)
                .OrderBy(g => g.Franchise.Name)
                .Search("Foo")
                .Take(8)
                .Skip(2)
                .Build().Send<HttpMockModel>(httpClient, "game").GetAwaiter().GetResult();

            var expected = "fields name,slug,follows,alternative_names,franchise.name;\n" +
                "where follows >= 3 & follows < 10 & cover.picture.url = \"https://gamecovers.com\"*;\n" +
                "sort name desc,franchise.name;\n" +
                "search \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, response.First().RequestBody);
        }

        [Test]
        public void SendQueryWithSearchInFieldSnakeCaseTest()
        {
            var response = new RequestBuilder<Game>()
                .Select<GameShort>()
                .Where(g => g.Follows >= 3 && g.Follows < 10)
                .OrderByDescending(g => g.Name)
                .Search("Foo", g => g.Name)
                .Take(8)
                .Skip(2)
                .Build().Send<HttpMockModel>(httpClient, "game").GetAwaiter().GetResult();

            var expected = "fields name,slug,follows,alternative_names,franchise.name;\n" +
                "where follows >= 3 & follows < 10;\n" +
                "sort name desc;\n" +
                "search name \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, response.First().RequestBody);
        }

        [Test]
        public void SendQueryWithSearchInFieldPascalCaseTest()
        {
            var response = new RequestBuilder<Game>(new Configuration.RequestBuilderConfiguration { CaseContract = Configuration.CaseContract.PascalCase })
                .Select<GameShort>()
                .Where(g => g.Follows >= 3 && g.Follows < 10)
                .OrderByDescending(g => g.Name)
                .Search("Foo", g => g.Name)
                .Take(8)
                .Skip(2)
                .Build().Send<HttpMockModel>(httpClient, "game").GetAwaiter().GetResult();

            var expected = "fields Name,Slug,Follows,AlternativeNames,Franchise.Name;\n" +
                "where Follows >= 3 & Follows < 10;\n" +
                "sort Name desc;\n" +
                "search Name \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, response.First().RequestBody);
        }

        [Test]
        public void SendQueryWithSearchInFieldCamelCaseTest()
        {
            var response = new RequestBuilder<Game>(new Configuration.RequestBuilderConfiguration { CaseContract = Configuration.CaseContract.CamelCase })
                .Select<GameShort>()
                .Where(g => g.Follows >= 3 && g.Follows < 10)
                .OrderByDescending(g => g.Name)
                .Search("Foo", g => g.Name)
                .Take(8)
                .Skip(2)
                .Build().Send<HttpMockModel>(httpClient, "game").GetAwaiter().GetResult();

            var expected = "fields name,slug,follows,alternativeNames,franchise.name;\n" +
                "where follows >= 3 & follows < 10;\n" +
                "sort name desc;\n" +
                "search name \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, response.First().RequestBody);
        }

        [Test]
        public void SendQueryWithSearchInMultiLevelFieldTest()
        {
            var response = new RequestBuilder<Game>()
                .Select<GameShort>()
                .Where(g => g.Follows >= 3 && g.Follows < 10)
                .OrderByDescending(g => g.Name)
                .Search("Foo", g => g.Franchise.Name)
                .Take(8)
                .Skip(2)
                .Build().Send<HttpMockModel>(httpClient, "game").GetAwaiter().GetResult();

            var expected = "fields name,slug,follows,alternative_names,franchise.name;\n" +
                "where follows >= 3 & follows < 10;\n" +
                "sort name desc;\n" +
                "search franchise.name \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, response.First().RequestBody);
        }
    }
}
