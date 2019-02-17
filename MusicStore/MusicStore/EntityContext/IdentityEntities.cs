using Microsoft.AspNet.Identity.EntityFramework;

namespace MusicStore.Models
{
    public class IdentityEntities : IdentityDbContext<ApplicationUser>
    {
        public IdentityEntities()
            : base("IdentitiesConnection")
        {
        }

        public static IdentityEntities Create()
        {
            return new IdentityEntities();
        }
    }
}