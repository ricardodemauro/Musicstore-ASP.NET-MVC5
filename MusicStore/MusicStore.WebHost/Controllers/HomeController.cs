using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicStore.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly string[] _migrations = new string[] { "artists", "genres", "albums" };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("migration/{step:int}")]
        public async Task<IActionResult> RunMigration([FromServices] IHostingEnvironment env, int step)
        {
            if (step >= _migrations.Length)
                return NoContent();

            var file = env.ContentRootFileProvider.GetFileInfo(Path.Combine("wwwroot", "data", $"{_migrations[step]}.json"));

            StringBuilder sb = new StringBuilder();
            if (file.Exists)
            {
                using (var stream = file.CreateReadStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    sb.AppendLine(await reader.ReadToEndAsync());
                }
            }

            return Content(sb.ToString());
        }

        [HttpGet]
        [Route("migration/{step:int}/do")]
        public async Task<IActionResult> RunMigrationPosted([FromServices] MusicStoreDbContext context, [FromServices] IHostingEnvironment env, int step, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (step >= _migrations.Length)
                return NoContent();

            var file = env.ContentRootFileProvider.GetFileInfo(Path.Combine("wwwroot", "data", $"{_migrations[step]}.json"));

            JArray array = null;
            if (file.Exists)
            {
                using (var stream = file.CreateReadStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                using (var jReader = new JsonTextReader(reader))
                {
                    array = await JArray.LoadAsync(jReader, cancellationToken);
                }
            }

            if (step == 0)
            {
                foreach (JObject item in array)
                {
                    string name = item.GetValue("name").Value<string>();

                    context.Genres.Add(new Genre() { Name = name });
                }
                await context.SaveChangesAsync();
            }

            return Content("Ok");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
