using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.WebHost.Data;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [Authorize]
    public class StoreManagerController : Controller
    {
        private readonly MusicStoreDbContext db;

        public StoreManagerController(MusicStoreDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // GET: /StoreManager/
        public async Task<IActionResult> Index([FromServices] IAlbumRepository albumRepository, CancellationToken cancellationToken)
        {
            var albums = await albumRepository.ToListWithDetails(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return View(albums);
        }

        // GET: /StoreManager/Details/5
        public async Task<IActionResult> Details([FromRoute]int? id, [FromServices] IAlbumRepository albumRepository, CancellationToken cancellationToken = default)
        {
            if (id == null)
                return BadRequest();

            Album album = await albumRepository.FindAlbum(id.Value, cancellationToken: cancellationToken);

            if (album == null)
                return NotFound();

            return View(album);
        }

        // GET: /StoreManager/Create
        public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
        {
            await BuildCreateView(cancellationToken);

            return View();
        }

        // POST: /StoreManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album, [FromServices] IAlbumRepository albumRepository, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                album = await albumRepository.Create(album, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                return RedirectToAction("Index");
            }
            await BuildCreateView(cancellationToken);

            return View(album);
        }

        // GET: /StoreManager/Edit/5
        public async Task<IActionResult> Edit([FromRoute] int? id, [FromServices] IAlbumRepository albumRepository, CancellationToken cancellationToken = default)
        {
            if (id == null)
                return BadRequest();

            Album album = await albumRepository.FindAlbum(id.Value, cancellationToken);
            if (album == null)
                return NotFound();

            await BuildCreateView(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            return View(album);
        }

        // POST: /StoreManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, [FromServices] IAlbumRepository albumRepository, [Bind("AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album, CancellationToken cancellation = default)
        {
            if (ModelState.IsValid)
            {
                await albumRepository.Update(album, cancellation);

                return RedirectToAction("Index");
            }

            await BuildCreateView(cancellation);

            return View(album);
        }

        // GET: /StoreManager/Delete/5
        public async Task<IActionResult> Delete([FromRoute] int? id, [FromServices] IAlbumRepository albumRepository, CancellationToken cancellationToken = default)
        {
            if (id == null)
                return BadRequest();

            Album album = await albumRepository.FindAlbum(id.Value, cancellationToken: cancellationToken);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        // POST: /StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int? id, [FromServices] IAlbumRepository albumRepository, CancellationToken cancellationToken = default)
        {
            if (id == null)
                return BadRequest();

            Album album = await albumRepository.FindAlbum(id.Value, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            await albumRepository.Delete(album);

            return RedirectToAction("Index");
        }

        protected async Task BuildCreateView(CancellationToken cancellationToken = default)
        {
            ViewBag.ArtistId = new SelectList(await db.Artists.ToListAsync(cancellationToken), "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(await db.Genres.ToListAsync(cancellationToken), "GenreId", "Name");
        }
    }
}
