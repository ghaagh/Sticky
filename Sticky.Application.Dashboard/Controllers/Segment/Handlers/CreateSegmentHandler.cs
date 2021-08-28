using MediatR;
using Microsoft.AspNetCore.Http;
using Sticky.Application.Dashboard.Controllers.Segment.Dto;
using Sticky.Application.Dashboard.Exceptions;
using Sticky.Application.Dashboard.Extensions;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Sticky.Application.Dashboard.Controllers.Segment.Handlers
{
    public class CreateSegmentHandler : IRequestHandler<CreateSegmentRequest, SegmentResponse>
    {
        private readonly IHostRepository _hostRepo;
        private readonly IUnitOfWork _saver;
        private readonly HttpContext _httpContext;
        public CreateSegmentHandler(IHostRepository hostRepo, IUnitOfWork saver, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _hostRepo = hostRepo;
            _saver = saver;
        }

        public async Task<SegmentResponse> Handle(CreateSegmentRequest request, CancellationToken cancellationToken)
        {
            var host = await _hostRepo.GetByIdAsync(request.HostId);
            if (!host.IsAssinedTo(_httpContext.User.GetUserId()))
                throw new NoAccessToHostException();

            var segment = new Domain.SegmentAggrigate.Segment(request.SegmentName,
                request.ActivityType, request.ActionType,
                request.ActivityExtra, request.ActionExtra);
            host.AddSegment(segment);
            await _saver.CommitAsync();

            return segment.ToSegmentResponse();
        }

    }
}
