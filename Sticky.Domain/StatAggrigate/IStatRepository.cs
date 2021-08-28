using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Domain.StatAggrigate
{
    public interface IStatRepository
    {
        Task IncreaseStatAsync(StatTypeEnum statTypeEnum, string increaseParameter);
    }
}
