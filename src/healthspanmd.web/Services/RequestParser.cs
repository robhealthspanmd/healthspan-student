using healthspanmd.core.Services.Authorization;
using Microsoft.AspNetCore.Http;

namespace healthspanmd.web.Services
{
    public class RequestParser : IRequestParser
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestParser(
            IHttpContextAccessor contextAccessor
            )
        {
            _contextAccessor = contextAccessor;
        }

        public int? GetUserId()
        {
            // /Analytics
            // /Analytics/Progress/{userId}
            var parts = _contextAccessor.HttpContext.Request.Path.ToString().Split('/');
            if (parts.Length == 1)
                return default;

            int idPosition = 0;
            if (parts.Length > 0)
                idPosition = string.IsNullOrEmpty(parts[0]) ? 3 : 2;

            if (parts.Length > idPosition)
                if (int.TryParse(parts[idPosition], out var userId))
                    return userId;


            return default;
        }
    }
}
