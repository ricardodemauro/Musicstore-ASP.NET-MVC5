using MusicStore.SiteMap.Models;

namespace MusicStore.SiteMap
{
    public abstract class SiteMapProvider
    {
        public abstract SiteMapNode SiteMap { get; }
    }
}
