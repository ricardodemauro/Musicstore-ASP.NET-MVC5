using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Infrastructure
{
    public interface IClaimsPrincipalProvider
    {
        ClaimsPrincipal Principal { get; }
    }
}
