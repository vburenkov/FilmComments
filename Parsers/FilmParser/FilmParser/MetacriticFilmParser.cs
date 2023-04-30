using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public class MetacriticFilmParser : IFilmCommentsParser
    {
        public List<Comment> GetFilmComments(Film f)
        {
            using (ChromeDriver d = ChromeHelper.GetDriver())
            {
                GoToSite(d);
                var comments = GetComments(d);
                return comments.Select(c => new Comment() { Text = c}).ToList();
            }
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

        private static void Wait(int secs)
        {
            Thread.Sleep(TimeSpan.FromSeconds(secs));
        }

        private static void GoToSite(ChromeDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.metacritic.com/movie/home-alone/user-reviews?sort-by=date&num_items=100");
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
