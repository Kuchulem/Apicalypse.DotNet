using Apicalypse.DotNet.Tests.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apicalypse.DotNet.Tests.Mocks
{
    class HttpMessageHandlerMock : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(new[] {new HttpMockModel
                {
                    RequestBody = await request.Content.ReadAsStringAsync()
                }})),
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
