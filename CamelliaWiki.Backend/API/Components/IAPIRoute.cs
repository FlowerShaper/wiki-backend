using JetBrains.Annotations;

namespace CamelliaWiki.Backend.API.Components;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IAPIRoute
{
    /// <summary>
    /// The path of the route. Must start with a slash.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// The HTTP method of the route.
    /// </summary>
    public HttpMethod Method { get; }

    /// <summary>
    /// Whether the route requires authentication.
    /// </summary>
    public bool RequiresAuthentication { get; }

    public void Handle(APIInteraction interaction);
}
