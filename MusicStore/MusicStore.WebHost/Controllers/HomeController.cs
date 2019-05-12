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
        public async Task<IActionResult> Index([FromServices] IAlbumRepository albumRepository)
        {
            // Get most popular albums
            var albums = await albumRepository.GetTopSellingAlbums(5);

            //todo verify a better place to put this
            if (albums == null || albums.Count == 0)
                return RedirectToAction(actionName: "Index", controllerName: "Home", routeValues: new { Area = "Setup" });

            return View(albums);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
