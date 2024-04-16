using System.Text;

namespace CamelliaWiki.Backend.Utils;

public static class StringUtils
{
    public static string Sanitize(this string input)
        => input.Replace("<", "&lt;").Replace(">", "&gt;")
                .Replace("&", "&amp;").Replace("\"", "&quot;")
                .Replace("'", "&apos;");

    public static string FormatToTitle(this string input)
    {
        input = input.ToLowerInvariant().Replace('-', ' ');

        var sb = new StringBuilder();
        var words = input.Split(' ');

        foreach (var word in words)
        {
            if (word.Length == 0)
                continue;

            sb.Append(char.ToUpper(word[0]));
            sb.Append(word[1..]);
            sb.Append(' ');
        }

        return sb.ToString().Trim();
    }
}
