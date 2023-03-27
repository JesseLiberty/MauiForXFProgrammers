using System.Security.Claims;

namespace ForgetMeNot.Api.Infrastructure
{
    public static class Extensions
    {
        public static Guid UserId(this HttpContext context)
        {
            try
            {
                return new Guid(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }
    }
}
