using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MusicStore.SiteMap.Models;
using MusicStore.SiteMap.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicStore.SiteMap
{
    public class FileSiteMapProvider : SiteMapProvider
    {
        private readonly string _fileName;

        private readonly IFileProvider _fileProvider;

        private readonly IUrlHelper _urlHelper;

        private static SiteMapNode _siteMap;

        private readonly object _lock = new object();

        public FileSiteMapProvider(IOptions<FileMapProviderOptions> options,
            IFileProvider fileProvider,
            IUrlHelper urlHelper)
        {
            _fileName = options.Value.File;
            _fileProvider = fileProvider;
            _urlHelper = urlHelper;
        }

        public override SiteMapNode SiteMap
        {
            get
            {
                if (_siteMap == null)
                {
                    lock (_lock)
                    {
                        if (_siteMap == null)
                        {
                            _siteMap = LoadAsync().GetAwaiter().GetResult();
                        }
                    }
                }
                return _siteMap;
            }
        }

        protected async virtual Task<SiteMapNode> LoadAsync()
        {
            var root = SiteMapNode.Empty;
            SiteMapNode node = root;

            var fInfo = _fileProvider.GetFileInfo(_fileName);
            if (fInfo.Exists)
            {
                XmlReaderSettings settings = new XmlReaderSettings()
                {
                    Async = true
                };
                using (var stream = new StreamReader(fInfo.CreateReadStream(), Encoding.UTF8))
                using (XmlReader xmlReader = XmlReader.Create(stream, settings))
                {
                    while (await xmlReader.ReadAsync().ConfigureAwait(false))
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            if ("mvcSiteMapNode".Eq(xmlReader.Name))
                            {
                                if (xmlReader.IsStartElement())
                                {
                                    if (!node.IsEmpty)
                                        node = node.CreateChildren();
                                }

                                node.Title = xmlReader.GetAttribute("title");
                                string action = xmlReader.GetAttribute("action");
                                string controller = xmlReader.GetAttribute("controller");
                                string area = xmlReader.GetAttribute("area");


                                node.Uri = _urlHelper.Action(action: action, controller: controller, values: new { area = area });

                                node.AuthorizationPolicy = xmlReader.GetAttribute("policy");
                                node.LocalizationKey = xmlReader.GetAttribute("loc");

                                if (xmlReader.IsEmptyElement)
                                    node = node.Parent;
                            }
                        }
                        else if (xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            if ("mvcSiteMapNode".Eq(xmlReader.Name) && node.Parent != null)
                                node = node.Parent;
                        }
                    }
                }
            }

            return root;
        }
    }
}
