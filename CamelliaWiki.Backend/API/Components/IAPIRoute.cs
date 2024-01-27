using System.Net;
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

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters);
}
