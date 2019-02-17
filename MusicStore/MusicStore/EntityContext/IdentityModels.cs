using Microsoft.AspNet.Identity.EntityFramework;

namespace MusicStore.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("IdentitiesConnection")
        {
        }
    }
}