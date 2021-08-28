using MediatR;
using Sticky.Domain.HostAggrigate;
using System.ComponentModel.DataAnnotations;

namespace Sticky.Application.Dashboard.Controllers.Host.Dto
{
    public class CreateHostRequest : IRequest<HostResponse>
    {
        [Required]
        public string HostAddress { get; set; }
        public ValidityEnum UserValidity { get; set; }
        public ValidityEnum ProductValidity { get; set; }
    }
}
