using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.Script.Exceptions
{
    public class CanNotFindHostFromRequestException:Exception
    {
        public CanNotFindHostFromRequestException() : base("can not find domain from request")
        {

        }
    }
}
