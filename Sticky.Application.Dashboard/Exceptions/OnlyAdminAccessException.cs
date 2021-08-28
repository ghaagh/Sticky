using System;

namespace Sticky.Application.Dashboard.Exceptions
{
    public class OnlyAdminAccessException: Exception
    {
        public OnlyAdminAccessException() : base("Only host admins can do this operation")
        {

        }
    }
}
