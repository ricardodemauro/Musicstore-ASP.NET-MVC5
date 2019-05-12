using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MusicStore.SiteMap.Localizations;
using MusicStore.SiteMap.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.SiteMap.Extensions.DependencyInjection
{
    public static class SiteMapServiceCollectionExtensions
    {
        public static IServiceCollection AddFileSiteMapProvider(this IServiceCollection services, string fileName)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }

            services.Configure<FileMapProviderOptions>(cfg => cfg.File = fileName);

            Register(services);

            return services;
        }

        public static IServiceCollection AddFileSiteMapProvider(this IServiceCollection services, Action<FileMapProviderOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            if (configure == null)
            {
                throw new ArgumentNullException("configure");
            }

            services.Configure(configure);

            Register(services);

            return services;
        }

        static void Register(IServiceCollection services)
        {
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.TryAddTransient<SiteMapProvider>(ctx =>
            {
                return new FileSiteMapProvider(ctx.GetService<IOptions<FileMapProviderOptions>>(),
                    ctx.GetService<IHostingEnvironment>().ContentRootFileProvider,
                    ctx.GetService<IUrlHelper>());
            });

            services.TryAddScoped<SiteMapLocalization>();

            RegisterViewProvider(services);
        }

        static void RegisterViewProvider(IServiceCollection services)
        {
            //Get a reference to the assembly that contains the view components
            var assembly = Assembly.GetExecutingAssembly();
            //Create an EmbeddedFileProvider for that assembly
            var embeddedFileProvider = new EmbeddedFileProvider(assembly);

            services.Configure<RazorViewEngineOptions>(opts => opts.FileProviders.Add(embeddedFileProvider));
        }
    }
}
