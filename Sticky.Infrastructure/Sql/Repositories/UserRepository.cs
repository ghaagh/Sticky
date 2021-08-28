using Microsoft.AspNetCore.Identity;
using Sticky.Domain.UserAggrigate;
using Sticky.Domain.UserAggrigate.Exceptions;
using System;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Sql.Repositories
{
    class UserRepository : IUserRepository
    {
        private readonly UserManager<IIdentity> _userManager;
        public UserRepository(UserManager<IIdentity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IIdentity> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user==null)
                throw new UserNotFoundException();
            return user;
        }

        public async Task<IIdentity> FindByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;

            //if (user == null)
            //    throw new UserNotFoundException();
            //return user;
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var user = new User()
            {
                Email = email,
                UserName = email,
            };
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;

        }
    }
}
