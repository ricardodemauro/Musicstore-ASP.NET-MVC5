using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MusicStore.SiteMap.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.SiteMap.Localizations
{
    public class SiteMapLocalization
    {
        private readonly IStringLocalizer _stringLocalizer;

        public SiteMapLocalization(IOptions<FileMapProviderOptions> options, IStringLocalizerFactory localizerFactory)
        {
            if (options.Value.UseLocalization)
                _stringLocalizer = localizerFactory.Create(options.Value.ResourceName, Assembly.GetEntryAssembly().FullName);
        }

        public IStringLocalizer Localizer { get { return _stringLocalizer; } }
    }
}
