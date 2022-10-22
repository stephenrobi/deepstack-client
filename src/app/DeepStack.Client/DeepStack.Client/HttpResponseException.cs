using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DeepStack.Client
{
    public class HttpResponseException
        : Exception
    {
        public HttpResponseException(string message)
            : base(message)
        {

        }

        public HttpResponseException(string message, HttpStatusCode httpStatusCode)
            : base(message)
        {
            this.StatusCode = httpStatusCode;
        }

        public HttpResponseException(string message, HttpStatusCode httpStatusCode, Exception innerException)
            : base(message, innerException)
        {
            this.StatusCode = httpStatusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }

    }
}
