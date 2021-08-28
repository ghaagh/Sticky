namespace Sticky.Application.Dashboard.Controllers.Account.Dto
{
    public class TokenResponse
    {
        public TokenResponse(string token)
        {
            Token = token;
        }
        public string Token { get; private set; }
    }
}
