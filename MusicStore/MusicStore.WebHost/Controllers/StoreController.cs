using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly MusicStoreDbContext storeDB;

        public StoreController(MusicStoreDbContext storeDB)
        {
            this.storeDB = storeDB;
        }

        //
        // GET: /Store/
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var genres = await storeDB.Genres.ToListAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return View(genres);
        }
        //
        // GET: /Store/Browse
        [Route("[controller]/browse/{genreName}")]
        public async Task<IActionResult> Browse([FromRoute] string genreName, [FromServices] IGenreRepository genreRepository, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<Album> albums = await genreRepository.AlbumsFromGenre(genreName, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            ViewData["GenreName"] = genreName;

            return View(albums);
        }
        //
        // GET: /Store/Details
        public async Task<IActionResult> Details([FromRoute] int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
                return BadRequest();

            Album album = await storeDB.Albums.FindAsync(id, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            return View(album);
        }

        //
        // GET: /Store/GenreMenu
        //[ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = storeDB.Genres.ToList();
            return PartialView(genres);
        }
    }
}