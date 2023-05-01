﻿using FilmParser;

IFilmCommentsParser parser = new MetacriticFilmParser();
Film f = parser.GetFilmByName("Rambo: First Blood Part II");
var comments = parser.GetFilmComments(f);
foreach (var comment in comments)
{
    Console.WriteLine(comment);
    Console.WriteLine("=========================");
}