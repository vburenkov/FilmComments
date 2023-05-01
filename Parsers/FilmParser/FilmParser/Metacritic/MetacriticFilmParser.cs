using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FilmParser
{
    public class MetacriticFilmParser : IFilmCommentsParser
    {
        public List<Comment> GetFilmComments(Film f)
        {
            using (ChromeDriver d = ChromeHelper.GetDriver())
            {
                GoToSite(d, GetFilmReviewsUrl(f.FilmSiteName));
                var comments = GetComments(d);
                return comments.Select(c => new Comment() { Text = c }).ToList();
            }
        }

        public Film GetFilmByName(string name)
        {
            using (ChromeDriver d = ChromeHelper.GetDriver())
            {
                GoToSite(d, GetFilmSearchUrl(name));
                Wait(2);
                ClosePrimarySearch(d);
                Wait(2);
                var shortFilmName = GetFilmShortName(d);

                var film = new Film() { FilmSiteName = shortFilmName };
                return film;
            }
        }     

        private static string GetFilmReviewsUrl(string shortFilmName)
        {
            return $"https://www.metacritic.com/movie/{shortFilmName}/user-reviews";
        }

        private static string GetFilmSearchUrl(string filmName)
        {
            return $"https://www.metacritic.com/search/movie/{filmName}/results";
        }

        private static string GetFilmShortName(ChromeDriver driver)
        {
            var firstFilm = driver.FindElements(By.CssSelector("div.title_space")).FirstOrDefault();
            if (firstFilm != null)
            {
                var inner = GetInnerAhref(driver, firstFilm);
                var filmShortName = GetFilmShortName(inner);
                return filmShortName;
            }

            throw new Exception("Can not get film short name");
        }

        private static void ClosePrimarySearch(ChromeDriver driver)
        {
            var closeSearch = driver.FindElements(By.CssSelector("div.primary_search_close")).FirstOrDefault();
            if (closeSearch != null)
            {
                closeSearch.Click();
            }            
        }

        private static string GetFilmShortName(string href)
        {
            return href.Split('/').Last();
        }

        private static List<String> GetComments(ChromeDriver driver)
        {
            var commentBlocks = driver.FindElements(By.CssSelector(".review.user")).ToList();
            var strComments = commentBlocks.Select(c => ProcessCommentBlock(driver, c)).ToList();
            return strComments;
        }

        private static string ProcessCommentBlock(ChromeDriver d, IWebElement e)
        {
            var collapsedText = e.FindElements(By.CssSelector("span.blurb.blurb_expanded")).FirstOrDefault();
            
            if (collapsedText != null)
            {
                return GetInnerHTML(d, collapsedText);
            }
            else 
            {
                var simpleText = e.FindElements(By.CssSelector("span")).FirstOrDefault();
                return simpleText.Text;
            }
        }

        private static string GetInnerHTML(ChromeDriver d, IWebElement e)
        { 
            IJavaScriptExecutor js = d as IJavaScriptExecutor;
            string innerHtml = (string)js.ExecuteScript("return arguments[0].innerHTML;", e);
            return innerHtml;
        }

        private static string GetInnerAhref(ChromeDriver d, IWebElement e)
        {
            IJavaScriptExecutor js = d as IJavaScriptExecutor;
            string innerHtml = (string)js.ExecuteScript("return arguments[0].getElementsByTagName('a')[0].getAttribute('href');", e);
            return innerHtml;
        }

        private static void Wait(int secs)
        {
            Thread.Sleep(TimeSpan.FromSeconds(secs));
        }

        private static void GoToSite(ChromeDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        private static void ScrollViaScript(ChromeDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var res = js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }

        private static void ScrollToEnd(ChromeDriver driver, Predicate<ChromeDriver> isFooterVisible)
        {
            while (!isFooterVisible(driver))
            {
                driver.FindElement(By.TagName("body")).Click();
                driver.FindElement(By.TagName("body")).SendKeys(Keys.PageDown);
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }        
    }
}
