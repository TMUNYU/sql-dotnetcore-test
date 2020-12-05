using System;
using System.Runtime.Serialization;

namespace Orders.Api.Services.Exceptions
{
    public class CustomerIdentityInvalidException : Exception
    {
        public CustomerIdentityInvalidException() { }
        public CustomerIdentityInvalidException(string message) : base(message) { }
        public CustomerIdentityInvalidException(string message, Exception inner) : base(message, inner) { }
        protected CustomerIdentityInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
