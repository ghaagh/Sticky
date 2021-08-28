using MediatR;
using Microsoft.AspNetCore.Http;
using Sticky.Application.Dashboard.Controllers.Partner.Dto;
using Sticky.Domain.PartnerAggrigate;
using Sticky.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Partner.Handlers
{
    public class CreatePartnerHandler : IRequestHandler<CreatePartnerRequest, PartnerResponse>
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IUnitOfWork _saver;
        private readonly HttpContext _httpContext;

        public CreatePartnerHandler(IPartnerRepository partnerRepository, IUnitOfWork saver, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _partnerRepository = partnerRepository;
            _saver = saver;
        }

        public async Task<PartnerResponse> Handle(CreatePartnerRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.GetUserId();
            var partner = new Domain.PartnerAggrigate.Partner(userId, request.Name, request.Domain, request.CookieMatchAddress);
            await _partnerRepository.CreatePartnerAsync(partner);
            await _saver.CommitAsync();
            return new PartnerResponse()
            {
                CookieMatchDomain = partner.CookieSyncAddress,
                HashCode = partner.Hash,
                Domain = partner.Domain,
                Id = partner.Id,
                Name = partner.Name
            };
        }
    }
}
