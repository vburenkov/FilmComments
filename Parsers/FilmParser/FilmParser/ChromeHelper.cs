using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public class ChromeHelper
    {
        public static bool IsWebElementWithAllAttributes(IWebElement e, string attrName, string csvAttrs)
        {
            var tokens = csvAttrs.Split();
            var classAttr = e.GetAttribute(attrName);

            foreach (var token in tokens)
            {
                if (!classAttr.Contains(token))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsWebElementWithAllAttributesFast(IWebElement e, string attrName, string csvAttrs)
        {
            var classAttr = e.GetAttribute(attrName);
            return classAttr.Contains(csvAttrs);
        }

        public static ChromeDriver GetDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.EnableMobileEmulation("Nexus 5");
            options.AddArgument("no-sandbox");
            options.AddArguments("disable-blink-features=AutomationControlled");
            options.AddArgument("remote-debugging-port=9222");

            ChromeDriver drv = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
            drv.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(20));

            return drv;
        }

        public static ChromeDriver AttachToRunning()
        {
            ChromeDriver result = null;
            ChromeOptions options = new ChromeOptions();
            options.DebuggerAddress = "127.0.0.1:9222";

            var policy = Policy
            .Handle<InvalidOperationException>()
            .WaitAndRetry(10, t => TimeSpan.FromSeconds(1));

            policy.Execute(() =>
            {
                result = new ChromeDriver(options);
            });

            return result;
        }
    }
}
