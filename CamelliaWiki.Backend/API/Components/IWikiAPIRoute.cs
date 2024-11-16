using JetBrains.Annotations;
using Midori.API.Components;

namespace CamelliaWiki.Backend.API.Components;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IWikiAPIRoute : IAPIRoute<WikiAPIInteraction>
{
}
