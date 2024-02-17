namespace CamelliaWiki.Backend.Utils;

public static class StringUtils
{
    public static string Sanitize(this string input) => input.Replace("<", "&lt;")
                                                             .Replace(">", "&gt;")
                                                             .Replace("&", "&amp;")
                                                             .Replace("\"", "&quot;")
                                                             .Replace("'", "&apos;");
}
