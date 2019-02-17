using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MusicStore.Infrastructure.HtmlHelpers
{
    public static class AlbumHtmlHelper
    {
        public static MvcHtmlString AlbumLink(this HtmlHelper helper, Album album, string action, string controller)
        {
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            string link = urlHelper.Action(action, controller, new { id = album.AlbumId });

            string value = $"<a href=\"{link}\">" +
                   $"<img alt=\"{album.Title}\" src=\"{album.AlbumArtUrl}\" />" +
                      $"<span>{album.Title}</span>" +
                  "</a>";

            return new MvcHtmlString(value);
        }
    }
}
