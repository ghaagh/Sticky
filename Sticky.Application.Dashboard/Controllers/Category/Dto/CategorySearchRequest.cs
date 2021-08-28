using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sticky.Application.Dashboard.Controllers.Category.Dto
{
    public class CategorySearchRequest : IRequest<IEnumerable<CategoryResponse>>
    {
        [Required]
        public long HostId { get; set; }
        [Required]
        public string Keyword { get; set; }
    }
}
