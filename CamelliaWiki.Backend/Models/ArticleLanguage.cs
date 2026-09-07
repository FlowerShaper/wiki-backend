using System.ComponentModel;

namespace CamelliaWiki.Backend.Models;

// ReSharper disable InconsistentNaming
public enum ArticleLanguage
{
    [Description("English")]
    en,

    [Description("Japanese")]
    jp,

    [Description("Spanish")]
    es,

    [Description("Italian")]
    it,

    [Description("Korean")]
    ko,

    [Description("Russian")]
    ru,

    [Description("Tagalog")]
    tl,

    [Description("Simplified Chinese")]
    zh_cn,

    [Description("Traditional Chinese")]
    zh_tw
}
