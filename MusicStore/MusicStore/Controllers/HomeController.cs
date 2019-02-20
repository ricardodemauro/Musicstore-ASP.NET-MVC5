using MusicStore.EntityContext;
using MusicStore.Models;
using MusicStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAlbumRepository albumRepository;

        public HomeController() : this(new AlbumEFRepository())
        {

        }

        public HomeController(IAlbumRepository albumRepository)
        {
            this.albumRepository = albumRepository;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);
            return View(albums);
        }
        public List<Album> GetTopSellingAlbums(int count)
        {
            return albumRepository.GetTopSellingAlbums(count);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page";
            return View();
        }
    }
}