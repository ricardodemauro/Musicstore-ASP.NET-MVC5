using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStore.WebHost.Models
{
    public class Cart
    {
        public int RecordId { get; set; }
        
        public string CartId { get; set; }

        public int AlbumId { get; set; }

        public int Count { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Album Album { get; set; }
    }
}