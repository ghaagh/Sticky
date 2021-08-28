using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Domain.UserAggrigate.Exceptions
{
    public class UserPassIncorrectException: Exception
    {
        public UserPassIncorrectException() : base("User or password is wrong")
        {

        }
    }
}
