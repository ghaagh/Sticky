using System;
namespace Sticky.Domain.UserAggrigate.Exceptions
{
    public class UserNotFoundException: Exception
    {
        public UserNotFoundException() : base("User was not found")
        {

        }
    }
}
