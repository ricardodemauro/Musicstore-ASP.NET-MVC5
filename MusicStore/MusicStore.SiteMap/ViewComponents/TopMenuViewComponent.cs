using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.SiteMap.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        private readonly SiteMapProvider _sitemapProvider;

        public TopMenuViewComponent(SiteMapProvider sitemapProvider)
        {
            _sitemapProvider = sitemapProvider ?? throw new ArgumentNullException(nameof(sitemapProvider));
        }

        public IViewComponentResult Invoke(string ulClass, string liClass, string aClass)
        {
            ViewData["ulClass"] = ulClass;
            ViewData["liClass"] = liClass;
            ViewData["aClass"] = aClass;

            return View(_sitemapProvider.SiteMap);
        }
    }
}
