using System;
using System.Text.RegularExpressions;

using OpenQA.Selenium;

namespace WikiCounter
{
    /// <summary>
    /// Helper methods to work with links.
    /// </summary>
    public class LinkHelper
    {
        private readonly IWebDriver _webDriver;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkHelper"/>.
        /// </summary>
        /// <param name="webDriver">Instance of the web driver.</param>
        public LinkHelper(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        /// <summary>
        /// Return <c>true</c>, if the <paramref name="paragraphLink"/> within <paramref name="paragraph"/> satisfies all conditions.
        /// </summary>
        /// <param name="paragraph">Paragraph web element with link(s).</param>
        /// <param name="paragraphLink">Link web element to potentially navigate to.</param>
        public bool LinkSatisfiesConditions(IWebElement paragraph, IWebElement paragraphLink)
        {
            var link = paragraphLink.GetAttribute(HtmlAttributes.HREF_HTML_ATTRIBUTE);
            var trimmedLink = TrimUrlParamsAndAnchors(link);

            return !(IsSamePageLink(trimmedLink)
                  || IsExternalLink(trimmedLink)
                  || IsRedLink(link)
                  || IsPronounciationLink(link)
                  || IsListeningLink(link)
                  || IsEnclosedByParenthesis(paragraph.Text, paragraphLink.Text)
            );
        }

        /// <summary>
        /// Returns <c>true</c>, if <paramref name="link"/> is a link to a sound recording.
        /// </summary>
        /// <param name="link">Link to check.</param>
        public bool IsListeningLink(string link) =>
            link.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Returns <c>true</c>, if <paramref name="link"/> is a link to pronunciation explanation.
        /// </summary>
        /// <param name="link">Link to check.</param>
        public bool IsPronounciationLink(string link) =>
            link.Contains(UrlParams.URL_PRONOUNCIATION_LINK, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Return <c>true</c>, if <paramref name="linkText"/> within <paramref name="paragraphText"/> is enclosed in parentheses.
        /// </summary>
        /// <param name="paragraphText">Text of the wikipedia paragraph.</param>
        /// <param name="linkText">Link text.</param>
        public bool IsEnclosedByParenthesis(string paragraphText, string linkText)
        {
            var link = new Regex($"{linkText}", RegexOptions.None, TimeSpan.FromSeconds(1));

            if (link.Matches(linkText).Count == 1)
            {
                var regex = new Regex($"\\(.*\\b{linkText.TrimInvalidCharacters()}\\b.*\\)", RegexOptions.None, TimeSpan.FromSeconds(5));

                return regex.IsMatch(paragraphText);
            }

            // TODO more occurrences check

            return false;
        }

        /// <summary>
        /// Trims anchors and query parameters from <paramref name="link"/>.
        /// </summary>
        /// <param name="link">Link to sanitize.</param>
        public string TrimUrlParamsAndAnchors(string link) =>
            link.TrimEnd('#').TrimEnd('&');

        /// <summary>
        /// Returns <c>true</c>, if the link points to the same page.
        /// </summary>
        /// <param name="link">Link to check.</param>
        public bool IsSamePageLink(string link) =>
            link.StartsWith(_webDriver.Url, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Returns <c>true</c>, if <paramref name="link"/> points outside of wikipedia.
        /// </summary>
        /// <param name="link">Link to check.</param>
        public bool IsExternalLink(string link) =>
            !link.StartsWith(Locators.WIKIPEDIA_URL, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Returns <c>true</c>, if <paramref name="link"/> points to an empty Wikipedia page.
        /// </summary>
        /// <param name="link">Link to check.</param>
        public bool IsRedLink(string link) =>
            link.Contains(UrlParams.URL_REDLINK_PARAM, StringComparison.OrdinalIgnoreCase);
    }
}
