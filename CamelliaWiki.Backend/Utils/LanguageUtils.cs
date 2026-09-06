using CamelliaWiki.Backend.Models;

namespace CamelliaWiki.Backend.Utils;

public static class LanguageUtils
{
    public static bool TryParse(string? lang, out ArticleLanguage language)
    {
        if (lang == null)
        {
            language = ArticleLanguage.en;
            return true;
        }

        return Enum.TryParse(lang, true, out language);
    }
}
