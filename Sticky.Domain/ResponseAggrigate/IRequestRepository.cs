using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Domain.ResponseAggrigate
{
    public interface IRequestRepository
    {
        Task<string> GetLast(ResponseUpdaterTypeEnum type, string exludedId = "");
        Task EnqueRequest(long request, ResponseUpdaterTypeEnum type, string excludedId="");
    }
}
