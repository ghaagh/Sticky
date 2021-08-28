using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Sticky.Application.Dashboard.Controllers.Partner.Dto
{
    public class ModifyPartnerRequest : IRequest<PartnerResponse>
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Name { get; set; }
        public string CookieMatchAddress { get; set; }
        public string Domain { get; set; }
    }
}
