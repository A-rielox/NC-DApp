using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}

// ClaimTypes.NameIdentifier)?.Value;
//  new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),

// ClaimTypes.Name)?.Value;
//  new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),