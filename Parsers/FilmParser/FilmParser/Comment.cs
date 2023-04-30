using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public class Comment
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
