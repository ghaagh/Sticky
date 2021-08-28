using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Sticky.Application.Dashboard.Controllers.Partner.Dto
{
    public class CreatePartnerRequest:IRequest<PartnerResponse>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string CookieMatchAddress { get; set; }
        [Required]
        public string Domain { get; set; }
    }
}
