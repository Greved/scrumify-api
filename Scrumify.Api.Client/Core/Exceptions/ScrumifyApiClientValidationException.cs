using System;
using System.Collections.Generic;

namespace Scrumify.Api.Client.Core.Exceptions
{
    public class ScrumifyApiClientValidationException : Exception
    {
        public List<string> ValidationMessages { get; }

        public ScrumifyApiClientValidationException(List<string> validationMessages): base(string.Join("; ", validationMessages))
        {
            ValidationMessages = validationMessages;
        }
    }
}