using Microsoft.AspNetCore.Http;
using Sticky.Application.Script.Exceptions;
using System;

namespace Sticky.Application.Script.Services
{
    public class Utility : IUtility
    {
        public string ExtractDomain(string origin)
        {
            if (string.IsNullOrEmpty(origin))
                throw new CanNotFindHostFromRequestException();
            var host = new Uri(origin).Host.ToLower();
            string topDomain;
            if (host.IndexOf(".") == host.LastIndexOf("."))
                topDomain = host;
            else
                topDomain = host.Substring(host.IndexOf(".") + 1);
            return topDomain;
        }
    }
}
