namespace WikiCounter
{
    /// <summary>
    /// Represents CSS classes of a Wikipedia page.
    /// </summary>
    public class Classes
    {
        public const string EMPTY_PARAGRAPH_CLASS = "mw-empty-elt";

        public const string CONTENT_TEXT_ID = "mw-content-text";

        public const string PARSER_OUTPUT_CLASS = "mw-parser-output";

        public const string FIRST_HEADING_ID = "firstHeading";
    }

    /// <summary>
    /// Represents URLs and element locators.
    /// </summary>
    public class Locators
    {
        public const string BROWSER_DRIVER_PATH = @"..\..\..\Drivers";

        public const string WIKIPEDIA_URL = "https://en.wikipedia.org";
    }

    /// <summary>
    /// Represents known Wikipedia URL query parameters.
    /// </summary>
    public class UrlParams
    {
        public const string URL_REDLINK_PARAM = "redlink=1";

        public const string URL_PRONOUNCIATION_LINK = "Help:IPA";
    }

    /// <summary>
    /// Represents HTML attributes.
    /// </summary>
    public class HtmlAttributes
    {
        public const string HREF_HTML_ATTRIBUTE = "href";
    }
}
