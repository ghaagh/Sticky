using MediatR;
using Microsoft.AspNetCore.Http;
using Sticky.Application.Dashboard.Controllers.Segment.Dto;
using Sticky.Application.Dashboard.Exceptions;
using Sticky.Application.Dashboard.Extensions;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.SegmentAggrigate;
using Sticky.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Segment.Handlers
{
    public class PublicToggleHandler : IRequestHandler<TogglePublicRequest,SegmentResponse>
    {
        private readonly IHostRepository _hostRepo;
        private readonly ISegmentRepository _segmentRepository;
        private readonly IUnitOfWork _saver;
        private readonly HttpContext _httpContext;
        public PublicToggleHandler(IHostRepository hostRepo,ISegmentRepository segmentRepository, IUnitOfWork saver, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _hostRepo = hostRepo;
            _segmentRepository = segmentRepository;
            _saver = saver;
        }

        public async Task<SegmentResponse> Handle(TogglePublicRequest request, CancellationToken cancellationToken)
        {
            var segment = await _segmentRepository.FindByIdAsync(request.SegmentId);
            var host = await _hostRepo.GetByIdAsync(segment.HostId);
            if (!host.IsAssinedTo(_httpContext.User.GetUserId()))
                throw new NoAccessToHostException();
            segment.TogglePublic();
            await _saver.CommitAsync();
            return segment.ToSegmentResponse();
        }
    }
}
