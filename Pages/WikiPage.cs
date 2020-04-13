using System;
using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace WikiCounter.Pages
{
    /// <summary>
    /// Represents a wikipedia page.
    /// </summary>
    public class WikiPage
    {
        private readonly IWebDriver _webDriver;
        private readonly LinkHelper _linkHelper;
        private HashSet<string> _visitedPages;

        /// <summary>
        /// Heading the current <see cref="WikiPage"/>.
        /// </summary>
        public string Heading => _webDriver.FindElement(By.Id(Classes.FIRST_HEADING_ID)).Text;

        /// <summary>
        /// Initializes a new instance of <see cref="WikiPage"/>.
        /// </summary>
        /// <param name="webDriver">Instance of the web driver.</param>
        public WikiPage(IWebDriver webDriver)
        {
            _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
            _linkHelper = new LinkHelper(webDriver);
            _visitedPages = new HashSet<string>();
        }

        /// <summary>
        /// Walks the Wikipedia page tree using the first links in the main text until it finds the 'Philosophy' page.
        /// </summary>
        /// <returns>Returns the number of steps (first link clicks) it took to find the 'Philosophy' page or -1 if a cycle occurred.</returns>
        public int FindPhilosophy()
        {
            NavigateToRandomWikiPage();

            int count = 0;

            while (!Heading.Equals("Philosophy", StringComparison.OrdinalIgnoreCase))
            {
                NavigateToFirstLink();

                if (_visitedPages.Contains(Heading))
                {
                    CloseBrowser();

                    return -1;
                }

                _visitedPages.Add(Heading);

                count++;
            }

            CloseBrowser();

            return count;
        }

        /// <summary>
        /// Navigates to a random Wikipedia page.
        /// </summary>
        private void NavigateToRandomWikiPage()
        {
            _webDriver.Url = Locators.WIKIPEDIA_URL;

            var builder = new Actions(_webDriver);

            // Press Alt + Shift + X for a random article
            builder.KeyDown(Keys.Alt)
                   .KeyDown(Keys.LeftShift)
                   .SendKeys("X")
                   .Perform();

            _visitedPages.Add(Heading);
        }

        /// <summary>
        /// Clicks the first non-parenthesized, non-italicized link in a Wikipedia main article.
        /// </summary>
        /// <remarks>
        /// Clicking ignores external links, links to the current page, or links to non-existent pages.
        /// The rules are described at <c>https://en.wikipedia.org/wiki/Wikipedia:Getting_to_Philosophy#Method_summarized</c>.
        /// </remarks>
        private void NavigateToFirstLink()
        {
            var content = _webDriver.FindElement(By.Id(Classes.CONTENT_TEXT_ID));
            var paragraphs = content.GetParagraphs();

            foreach (var paragraph in paragraphs)
            {
                var paragraphLinks = paragraph.GetLinks();

                if (!paragraphLinks.Any())
                {
                    continue;
                }

                var firstValidLink = GetFirstValidLink(paragraph, paragraphLinks);

                if (firstValidLink is null)
                {
                    continue;
                }

                _webDriver.Url = firstValidLink;

                return;
            }
        }

        /// <summary>
        /// Gets the first valid link to click within a Wikipedia page.
        /// </summary>
        /// <param name="paragraphLinks"></param>
        /// <returns></returns>
        private string? GetFirstValidLink(IWebElement paragraph, IEnumerable<IWebElement> paragraphLinks) =>
            paragraphLinks.FirstOrDefault(paragraphLink =>  _linkHelper.LinkSatisfiesConditions(paragraph, paragraphLink))
                         ?.GetAttribute(HtmlAttributes.HREF_HTML_ATTRIBUTE);

        /// <summary>
        /// Closes the browser window.
        /// </summary>
        private void CloseBrowser()
        {
            _webDriver.Close();
        }
    }
}
