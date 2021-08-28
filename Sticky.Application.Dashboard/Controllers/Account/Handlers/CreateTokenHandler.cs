using MediatR;
using Sticky.Application.Dashboard.Controllers.Account.Dto;
using Sticky.Application.Dashboard.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Account.Handlers
{
    public class CreateTokenHandler : IRequestHandler<GetTokenRequest, TokenResponse>
    {
        private readonly ITokenGenerator _tokenGenerator;
        public CreateTokenHandler(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }
        public async Task<TokenResponse> Handle(GetTokenRequest request, CancellationToken cancellationToken)
        {
            var token = await _tokenGenerator.GenerateAsync(request.Email, request.Password);
            return new TokenResponse(token);
        }
    }
}
