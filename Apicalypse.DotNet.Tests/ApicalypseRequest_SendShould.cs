using Apicalypse.DotNet.Tests.Mocks;
using Apicalypse.DotNet.Tests.Models;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;

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

            var expected = "fields name,slug,follows,alternative_names;\n" +
                "where follows >= 3 & follows < 10;\n" +
                "sort name desc;\n" +
                "search \"Foo\";\n" +
                "limit 8;\n" +
                "offset 2;";

            Assert.AreEqual(expected, response.First().RequestBody);
        }
    }
}
