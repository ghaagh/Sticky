using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Exceptions
{
    public class CurrentUserIsNotPartnerOwnerException: Exception
    {
        public CurrentUserIsNotPartnerOwnerException():base("Current user is not the owner of the partner to modify it")
        {

        }
    }
}
