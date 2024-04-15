using CamelliaWiki.Backend.Components;

namespace CamelliaWiki.Backend.Utils;

public static class LanguageUtils
{
    public static bool TryParse(string? lang, out Language language)
    {
        if (lang == null)
        {
            Logger.Log("Language not specified, defaulting to English");
            language = Language.en;
            return true;
        }

        return Enum.TryParse(lang, true, out language);
    }
}
