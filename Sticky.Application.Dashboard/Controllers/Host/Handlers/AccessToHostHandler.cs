using MediatR;
using Microsoft.AspNetCore.Http;
using Sticky.Application.Dashboard.Controllers.Host.Dto;
using Sticky.Application.Dashboard.Exceptions;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.Shared;
using Sticky.Domain.UserAggrigate;
using Sticky.Domain.UserAggrigate.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Host.Handlers
{
    public class AccessToHostHandler : IRequestHandler<AccessHostRequest, HostResponse>
    {
        private readonly IHostRepository _hostRepository;
        private readonly IUnitOfWork _saver;
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public AccessToHostHandler(IHostRepository hostRepository, IUserRepository userRepository, IUnitOfWork saver, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _hostRepository = hostRepository;
            _userRepository = userRepository;
            _saver = saver;
        }

        public async Task<HostResponse> Handle(AccessHostRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);

            var currentUserId = _httpContext.User.GetUserId();
            var host = await _hostRepository.GetByIdAsync(request.HostId);

            if (!host.IsOwnedBy(currentUserId))
                throw new OnlyAdminAccessException();

            host.AccessToUser(user.GetUserId(), request.Admin);
            await _saver.CommitAsync();
            return new HostResponse()
            {
                HostAddress = host.HostAddress
            };
        }
    }
}
