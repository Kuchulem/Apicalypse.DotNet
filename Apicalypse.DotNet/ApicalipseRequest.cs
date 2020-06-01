using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Apicalypse.DotNet
{
    /// <summary>
    /// A request used to send a query from the request builder to an API.
    /// </summary>
    public class ApicalipseRequest
    {
        private readonly string body;

        internal ApicalipseRequest(string body)
        {
            this.body = body;
        }

        /// <summary>
        /// Sends the query to an <em>endpoint</em> with the provided <em>HttpClient</em> and
        /// returns the corresponding HttpResponseMessage.
        /// </summary>
        /// <param name="httpClient">The HttpClient used to send the query</param>
        /// <param name="endpoint">The endpoint used for the POST request</param>
        /// <returns>HttpResponseMessage</returns>
        public Task<HttpResponseMessage> Send(HttpClient httpClient, string endpoint)
        {
            return httpClient.PostAsync($"/{endpoint}", new StringContent(body));
        }

        /// <summary>
        /// Sends the query to an <em>endpoint</em> with the provided <em>HttpClient</em> and
        /// returns an object mapped on the response content.
        /// </summary>
        /// <typeparam name="T">The class to map from the response content</typeparam>
        /// <param name="httpClient">The HttpClient used to send the query</param>
        /// <param name="endpoint">The endpoint used for the POST request</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Send<T>(HttpClient httpClient, string endpoint)
            where T : new()
        {
            var response = await Send(httpClient, endpoint).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<T>>(content);
            }

            throw new Exception($"Response status code is not successfull : {response.StatusCode} : {response.ReasonPhrase}");
        }
    }
}
