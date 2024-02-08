using System.Net;
using System.Reflection;
using System.Text;
using CamelliaWiki.Backend.API.Components;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API;

public class APIServer
{
    private readonly List<IAPIRoute> routeList = new();
    private HttpListener? listener { get; set; }

    public void Start(int port)
    {
        loadRoutes();

        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");

        if (!string.IsNullOrEmpty(Program.Config.Host))
            listener.Prefixes.Add(Program.Config.Host);

        listener.Start();

        var thread = new Thread(startListener);
        thread.Start();

        Logger.Log($"Started API server on port {port}.");
    }

    private void loadRoutes()
    {
        Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IAPIRoute)))
                .ToList()
                .ForEach(t =>
                {
                    var route = (IAPIRoute)Activator.CreateInstance(t)!;
                    routeList.Add(route);
                });

        routeList.Sort((a, b) => string.Compare(a.Path, b.Path, StringComparison.Ordinal));
        routeList.ForEach(r => Logger.Log($"Loaded API route {r.Method.Method} {r.Path}"));
    }

    private void startListener(object? o)
    {
        while (true)
            process();

        // ReSharper disable once FunctionNeverReturns
    }

    private void process()
    {
        var res = listener?.BeginGetContext(handle, listener);
        res?.AsyncWaitHandle.WaitOne();
    }

    private void handle(IAsyncResult result)
    {
        var context = listener?.EndGetContext(result);
        if (context == null) return;

        var req = context.Request;
        var res = context.Response;

        IAPIRoute? route = null;
        var parameters = new RequestParameters();

        foreach (var handler in routeList)
        {
            if (!string.Equals(req.HttpMethod, handler.Method.Method, StringComparison.InvariantCultureIgnoreCase)) continue;
            if (req.Url == null) continue; // don't even know how this would happen but ok

            var url = req.Url.AbsolutePath;

            if (handler.Path == url && !handler.Path.Contains(':'))
            {
                route = handler; // exact match with no parameters
                break;
            }

            var parts = handler.Path.Split('/');
            var reqParts = url.Split('/');

            if (parts.Length == 0 || reqParts.Length == 0)
                continue;

            if (reqParts.Last() == "")
                reqParts = reqParts[..^1]; // remove trailing slash (if any)

            if (parts.Length != reqParts.Length)
                continue;

            var match = true;
            var reqParams = new RequestParameters();

            for (var i = 0; i < parts.Length; i++)
            {
                if (parts[i].StartsWith(":"))
                {
                    reqParams.Add(parts[i][1..], reqParts[i]);
                }
                else if (!parts[i].Equals(reqParts[i]))
                {
                    match = false;
                    break;
                }
            }

            if (!match)
                continue;

            route = handler;
            parameters = reqParams;
        }

        APIResponse? response;

        if (route == null)
        {
            response = new APIResponse { Code = ErrorCodes.NotFound };
        }
        else
        {
            try
            {
                response = route.Handle(context, parameters);
            }
            catch (Exception e)
            {
                response = new APIResponse { Code = ErrorCodes.InternalError };
                Logger.Log(e);
            }
        }

        sendResponse(res, response);
    }

    private void sendResponse(HttpListenerResponse res, object obj)
    {
        var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        }));

        res.ContentLength64 = buffer.Length;
        res.ContentEncoding = Encoding.UTF8;
        res.AddHeader("Content-Type", "application/json");
        res.AddHeader("Access-Control-Allow-Origin", "*");
        res.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
        res.AddHeader("Access-Control-Allow-Headers", "*");
        res.OutputStream.Write(buffer);
        res.OutputStream.Close();
    }
}
