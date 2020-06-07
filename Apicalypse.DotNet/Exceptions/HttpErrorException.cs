using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;

namespace Apicalypse.DotNet.Exceptions
{
    public class HttpErrorException : Exception
    {
        const string MESSAGE = "Error on calling API with Apicalypse : ";

        /// <summary>
        /// The status code returned
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// The reason phrase provided
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Exception thrown when the HTTP call to the API failed
        /// </summary>
        /// <param name="httpResponse">The response from the call</param>
        public HttpErrorException(HttpResponseMessage httpResponse)
            : base($"{MESSAGE}{httpResponse.StatusCode} - {httpResponse.ReasonPhrase}")
        {
        }
    }
}
