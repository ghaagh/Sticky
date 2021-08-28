using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Sticky.Application.Dashboard.Controllers.Account.Dto
{
    public class GetTokenRequest:IRequest<TokenResponse>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
