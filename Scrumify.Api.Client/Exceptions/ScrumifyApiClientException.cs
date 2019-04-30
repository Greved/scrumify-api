using System;
using System.Runtime.Serialization;

namespace Scrumify.Api.Client.Exceptions
{
    public class ScrumifyApiClientException: Exception
    {
        public ScrumifyApiClientException()
        {
        }

        protected ScrumifyApiClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ScrumifyApiClientException(string message) : base(message)
        {
        }
    }
}