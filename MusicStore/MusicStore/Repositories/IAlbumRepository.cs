﻿using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Repositories
{
    public interface IAlbumRepository
    {
        List<Album> GetTopSellingAlbums(int count);
    }
}
