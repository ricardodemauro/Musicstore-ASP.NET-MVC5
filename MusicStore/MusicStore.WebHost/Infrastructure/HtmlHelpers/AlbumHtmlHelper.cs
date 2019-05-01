using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MusicStore.WebHost.Models;

namespace MusicStore.WebHost.Infrastructure.HtmlHelpers
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
