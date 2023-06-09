﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public interface IFilmCommentsParser
    {
        List<Comment> GetFilmComments(Film f);

        Film GetFilmByName(string name);
    }
}
