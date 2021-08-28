using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Sticky.Application.Dashboard.Controllers.Account.Dto
{
    public class RegisterRequest: IRequest<TokenResponse>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
