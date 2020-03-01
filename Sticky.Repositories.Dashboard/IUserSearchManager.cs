using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard
{
   public  interface IUserSearchManager
    {
       Task<string> SearchUser(string email);
    }
}
