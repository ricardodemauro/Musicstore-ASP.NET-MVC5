using Microsoft.AspNetCore.Mvc;
using MusicStore.WebHost.Models;
using MusicStore.WebHost.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.WebHost.Infrastructure.ViewComponents
{
    [ViewComponent(Name = "genre")]
    public class GenreViewComponent : ViewComponent
    {
        private readonly IGenreRepository _genreRepository;

        public GenreViewComponent(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Genre> genres = await _genreRepository.ToList();
            return View(genres);
        }
    }
}
