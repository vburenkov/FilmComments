using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public class Film
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public DateOnly ReleaseDate { get; set; }

        public string Url { get; set; }

        public string ReviewsUrl { get; set; }
    }
}
