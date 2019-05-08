using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace MusicStore.WebHost.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Collection<Album> Albums { get; set; }
    }
}