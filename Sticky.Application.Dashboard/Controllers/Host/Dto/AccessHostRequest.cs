using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Sticky.Application.Dashboard.Controllers.Host.Dto
{
    public class AccessHostRequest:IRequest<HostResponse>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int HostId { get; set; }
        public bool Admin { get; set; }

    }
}
