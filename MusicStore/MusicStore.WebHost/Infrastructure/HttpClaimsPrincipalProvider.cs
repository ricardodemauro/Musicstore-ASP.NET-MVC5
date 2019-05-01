using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Infrastructure
{
    public class HttpClaimsPrincipalProvider : IClaimsPrincipalProvider
    {
        private readonly IHttpContextAccessor _httpContext;

        public HttpClaimsPrincipalProvider(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ClaimsPrincipal Principal => _httpContext.HttpContext.User;
    }
}
