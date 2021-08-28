using Sticky.Domain.HostAggrigate.Exceptions;
using Sticky.Domain.UserAggrigate.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sticky.Application.Dashboard.Middlewares.ExceptionHandling
{
    public static class ExceptionMapper
    {
        private readonly static Dictionary<Type, HttpStatusCode> _mapper;

        static ExceptionMapper()
        {
            _mapper = new Dictionary<Type, HttpStatusCode>
            {
                { typeof(UserNotFoundException), HttpStatusCode.BadRequest },
                { typeof(UserPassIncorrectException), HttpStatusCode.BadRequest },
                { typeof(DuplicatedHostException), HttpStatusCode.BadRequest }
            };
        }
        public static HttpStatusCode Find(Exception exception)
        {
            if (_mapper.TryGetValue(exception.GetType(), out HttpStatusCode statusCode))
                return statusCode;
            return statusCode;
        }

    }
}
