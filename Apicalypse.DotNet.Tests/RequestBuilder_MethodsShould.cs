using Apicalypse.DotNet.Tests.Models;
using NUnit.Framework;

namespace Apicalypse.DotNet.Tests
{
    public class RequestBuilder_MethodsShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReturnBuilderOnSelectTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.Select<GameName>());
        }

        [Test]
        public void ReturnBuilderOnWhereTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.Where(g => g.Name == "Foo"));
        }

        [Test]
        public void ReturnBuilderOnExcludeTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.Exclude(g => g.Name));
        }

        [Test]
        public void ReturnBuilderOnOrderByTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.OrderBy(g => g.Name));
        }

        [Test]
        public void ReturnBuilderOnOrderByDescendingTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.OrderByDescending(g => g.Name));
        }

        [Test]
        public void ReturnBuilderOnSearchTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.Search("Foo"));
        }

        [Test]
        public void ReturnBuilderOnTakeTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.Take(20));
        }

        [Test]
        public void ReturnBuilderOnSkipTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<RequestBuilder<Game>>(builder.Skip(0));
        }
        [Test]
        public void ReturnRequestOnBuildTest()
        {
            var builder = new RequestBuilder<Game>();

            Assert.IsInstanceOf<ApicalipseRequest>(builder.Build());
        }

    }
}