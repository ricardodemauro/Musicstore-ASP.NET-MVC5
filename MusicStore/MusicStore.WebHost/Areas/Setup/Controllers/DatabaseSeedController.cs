using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Infrastructure;
using MusicStore.WebHost.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Areas.Setup.Controllers
{
    [Area("Setup")]
    [Authorize(Policy = PolicyNames.ADMIN)]
    public class DatabaseSeedController : Controller
    {
        private readonly string[] _migrations = new string[] { "artists", "genres", "albums" };

        public IActionResult Index()
        {
            return View(_migrations);
        }

        [HttpGet]
        public async Task<IActionResult> RunMigration([FromQuery]string migrationName, [FromServices] IHostingEnvironment env, CancellationToken cancellation = default)
        {
            if (!_migrations.Contains(migrationName))
                return NoContent();

            migrationName = _migrations.Single(x => x.Eq(migrationName));

            var file = env.ContentRootFileProvider.GetFileInfo(Path.Combine("wwwroot", "data", $"{migrationName}.json"));

            StringBuilder sb = new StringBuilder();
            if (file.Exists)
            {
                using (var stream = file.CreateReadStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    sb.AppendLine(await reader.ReadToEndAsync());
                }
            }

            cancellation.ThrowIfCancellationRequested();

            return Content(sb.ToString(), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [ActionName("migration")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RunMigrationPosted([FromQuery]string migrationName, [FromServices] MusicStoreDbContext context, [FromServices] IHostingEnvironment env, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!_migrations.Contains(migrationName))
                return NoContent();

            migrationName = _migrations.Single(x => x.Eq(migrationName));

            var file = env.ContentRootFileProvider.GetFileInfo(Path.Combine("wwwroot", "data", $"{migrationName}.json"));

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

            if (string.Compare("genres", migrationName, true) == 0)
            {
                List<Genre> genres = new List<Genre>();
                foreach (JObject item in array)
                {
                    string name = item.GetValue("name").Value<string>();

                    genres.Add(new Genre() { Name = name });
                }
                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
            }
            else if (string.Compare("artists", migrationName, true) == 0)
            {
                List<Artist> artists = new List<Artist>();
                foreach (JObject item in array)
                {
                    string name = item.GetValue("name").Value<string>();

                    artists.Add(new Artist() { Name = name });
                }
                await context.Artists.AddRangeAsync(artists);
                await context.SaveChangesAsync();
            }
            else if (string.Compare("albums", migrationName, true) == 0)
            {
                Dictionary<string, Genre> genres = new Dictionary<string, Genre>();
                Dictionary<string, Artist> artists = new Dictionary<string, Artist>();
                List<Album> albums = new List<Album>();

                foreach (JObject item in array)
                {
                    string name = item.GetValue("name").Value<string>();
                    decimal price = item.GetValue("price").Value<decimal>();
                    string albumUrl = item.GetValue("albumArtUrl").Value<string>();
                    string artist = item.GetValue("artist").Value<string>();
                    string genre = item.GetValue("genre").Value<string>();

                    if (!genres.ContainsKey(genre))
                    {
                        var genDb = await context.Genres.FirstOrDefaultAsync(x => x.Name == genre);
                        genres.Add(genre, genDb);
                    }
                    if (!artists.ContainsKey(artist))
                    {
                        var artDb = await context.Artists.FirstOrDefaultAsync(x => x.Name == artist);
                        artists.Add(artist, artDb);
                    }


                    albums.Add(new Album
                    {
                        Title = name,
                        Price = price,
                        AlbumArtUrl = albumUrl,
                        Genre = genres[genre],
                        Artist = artists[artist]
                    });
                }
                await context.Albums.AddRangeAsync(albums);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
