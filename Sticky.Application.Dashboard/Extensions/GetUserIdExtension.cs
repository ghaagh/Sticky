using System.Security.Claims;
public static class GetUserIdExtension
{
    public static string GetUserId(this ClaimsPrincipal claims)
    {
        var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
        var userId = claim.Value;
        return userId;
    }
}

