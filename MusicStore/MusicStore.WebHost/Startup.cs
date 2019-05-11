using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Infrastructure;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.Repositories;
using System;
using MusicStore.SiteMap.Extensions.DependencyInjection;

namespace MusicStore.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            string connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContextPool<MusicStoreDbContext>(opts => opts.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MusicStoreDbContext>()
                .AddDefaultUI(Microsoft.AspNetCore.Identity.UI.UIFramework.Bootstrap4)
                .AddDefaultTokenProviders();

            services.AddSession(opts =>
            {
                opts.Cookie.Name = Guid.NewGuid().ToString();
                opts.Cookie.HttpOnly = true;
                opts.Cookie.IsEssential = true;
                opts.IdleTimeout = TimeSpan.FromMinutes(3);
            });

            services.AddOptions();

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<ISessionProvider, HttpSessionProvider>();
            services.AddTransient<IClaimsPrincipalProvider, HttpClaimsPrincipalProvider>();

            //services.AddDirectoryBrowser();
            services.AddTransient<IAlbumRepository, EFAlbumRepository>();
            services.AddTransient<IGenreRepository, EFGenreRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();

            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddFileSiteMapProvider("wwwroot/mvc.sitemap");

            //services.Configure<FileMapProviderOptions>(ctx =>
            //{
            //    ctx.File = "wwwroot/mvc.sitemap";
            //});

            //services.AddScoped<SiteMapProvider>(ctx =>
            //{
            //    return new FileSiteMapProvider(ctx.GetService<IOptions<FileMapProviderOptions>>(),
            //        ctx.GetService<IHostingEnvironment>().ContentRootFileProvider,
            //        ctx.GetService<IUrlHelper>());
            //});

            //services.Configure<RazorViewEngineOptions>
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
