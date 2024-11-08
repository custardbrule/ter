using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Shared.Exceptions
{
    public class AppException : Exception
    {
        public readonly HttpStatusCode StatusCode;
        public readonly object Errors;
        public AppException(string message, HttpStatusCode statusCode, object errors) : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
