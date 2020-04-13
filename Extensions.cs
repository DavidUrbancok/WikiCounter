using System;
using System.Collections.Generic;

using OpenQA.Selenium;

namespace WikiCounter
{
    /// <summary>
    /// Extension methods for the <see cref="IWebElement"/> interface.
    /// </summary>
    public static class IWebElementExtensions
    {
        /// <summary>
        /// Gets all paragraphs within <paramref name="webElement"/>.
        /// </summary>
        /// <param name="webElement">Web element instance from which to get paragraphs.</param>
        public static IEnumerable<IWebElement> GetParagraphs(this IWebElement webElement)
        {
            if (webElement is null)
            {
                throw new ArgumentNullException(nameof(webElement));
            }

            return webElement.FindElements(By.XPath($"//div[@class='{Classes.PARSER_OUTPUT_CLASS}']" +
                                                    $"/p[not(@class='{Classes.EMPTY_PARAGRAPH_CLASS}')]"));
        }

        /// <summary>
        /// Gets all link within <paramref name="webElement"/>.
        /// </summary>
        /// <param name="webElement">Web element instance from which to get links.</param>
        public static IEnumerable<IWebElement> GetLinks(this IWebElement webElement)
        {
            if (webElement is null)
            {
                throw new ArgumentNullException(nameof(webElement));
            }

            return webElement.FindElements(By.TagName("a"));
        }
    }

    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Trims some special characters from the end of <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text to trim.</param>
        public static string TrimInvalidCharacters(this string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var charsToTrim = new char[] { '.', ',', ':', ';', '/', '\\', '-' };

            return text.TrimStart(charsToTrim).TrimEnd(charsToTrim);
        }
    }
}
