using MediatR;
using Sticky.Domain.UserAggrigate;
using System.Threading.Tasks;
using Sticky.Application.Dashboard.Controllers.Account.Dto;
using System.Threading;
using Sticky.Application.Dashboard.Services;

namespace Sticky.Application.Dashboard.Controllers.Account.Handlers
{
    public sealed class RegistrationHandler : IRequestHandler<RegisterRequest,TokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public RegistrationHandler(IUserRepository userRepository,ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<TokenResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            await _userRepository.RegisterAsync(request.UserName, request.Password);
            var token = await _tokenGenerator.GenerateAsync(request.UserName, request.Password);
            return new TokenResponse(token);

        }
    }
}
