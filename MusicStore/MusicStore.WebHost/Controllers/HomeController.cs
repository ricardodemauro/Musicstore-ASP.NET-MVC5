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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicStore.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly string[] _migrations = new string[] { "artists", "genres", "albums" };

        public async Task<IActionResult> Index([FromServices] IAlbumRepository albumRepository)
        {
            // Get most popular albums
            var albums = await albumRepository.GetTopSellingAlbums(5);
            return View(albums);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("[controller]/migration/{step:int}")]
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

            return Content(sb.ToString(), "application/json", Encoding.UTF8);
        }

        [HttpGet]
        [Route("[controller]/migration/{step:int}/do")]
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

            if (string.Compare("genres", _migrations[step], true) == 0)
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
            else if (string.Compare("artists", _migrations[step], true) == 0)
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
            else if (string.Compare("albums", _migrations[step], true) == 0)
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

            return Content("Ok");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
