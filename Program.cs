using System;

using WikiCounter.Pages;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace WikiCounter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var wikiPage = new WikiPage(new ChromeDriver(Locators.BROWSER_DRIVER_PATH));

            // To test different browsers, uncomment the following section and add a startup argument
            /*var wikiPage = new WikiPage(args[0] switch
            {
                "-chrome"  => new ChromeDriver(Config.BROWSER_DRIVER_PATH),
                "-firefox" => new FirefoxDriver(Config.BROWSER_DRIVER_PATH),
                _          => new ChromeDriver(Config.BROWSER_DRIVER_PATH)
            });*/

            var count = wikiPage.FindPhilosophy();

            if (count == -1)
            {
                Console.WriteLine("Pages cycle or there is no link in the text.");
            }
            else
            {
                Console.WriteLine($"Philosophy found in {count} steps.");
            }
        }
    }
}
