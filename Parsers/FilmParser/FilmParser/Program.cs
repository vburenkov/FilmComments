using FilmParser;

IFilmCommentsParser parser = new MetacriticFilmParser();
var comments = parser.GetFilmComments(new Film());
foreach (var comment in comments)
{
    Console.WriteLine(comment);
    Console.WriteLine("==================");
}