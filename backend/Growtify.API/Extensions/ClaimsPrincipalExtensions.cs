using System.Security.Claims;

namespace Growtify.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetMemberId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                id = user.FindFirstValue("nameid");
            }

            return id ?? throw new Exception("Cannot get memberId from token");
        }
    }
}
