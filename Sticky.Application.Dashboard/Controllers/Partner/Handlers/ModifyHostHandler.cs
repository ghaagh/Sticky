using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sticky.Application.Dashboard.Controllers.Partner.Dto;
using Sticky.Application.Dashboard.Exceptions;
using Sticky.Domain.PartnerAggrigate;
using Sticky.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Partner.Handlers
{
    public class ModifyPartnerHandler : IRequestHandler<ModifyPartnerRequest, PartnerResponse>
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IUnitOfWork _saver;
        private readonly HttpContext _httpContext;
        public ModifyPartnerHandler(IPartnerRepository partnerRepository, IUnitOfWork saver, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _partnerRepository = partnerRepository;
            _saver = saver;
        }

        public async Task<PartnerResponse> Handle(ModifyPartnerRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.GetUserId();
            var partner = await _partnerRepository.GetPartnerAsync(request.Id);
            if (partner.UserId != userId)
                throw new CurrentUserIsNotPartnerOwnerException();

            partner.SetCookieAddress(request.CookieMatchAddress)
                .SetName(request.Name)
                .SetDomain(request.Domain);

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
