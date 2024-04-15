using System.Diagnostics;
using System.Net;
using System.Text;
using CamelliaWiki.Backend.Components.Users;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API.Components;

public class APIInteraction
{
    private static readonly string[] allowed_methods = { "GET", "POST", "PUT", "DELETE", "OPTIONS" };

    private static readonly string[] allowed_headers =
    {
        "Content-Type",
        "Authorization",
        "X-Requested-With",
        "X-Forwarded-For"
    };

    public HttpListenerRequest Request { get; }
    public HttpListenerResponse Response { get; }
    public Dictionary<string, string> Parameters { get; }

    public IPAddress RemoteIP { get; }

    public ulong UserID { get; }
    public User User { get; } = null!;

    public bool IsAuthorized => UserID != 0ul;
    public ErrorCodes AuthError { get; }

    public APIInteraction(HttpListenerRequest req, HttpListenerResponse res, Dictionary<string, string> parameters)
    {
        Request = req;
        Response = res;
        Parameters = parameters;

        var forward = Request.Headers.Get("X-Forwarded-For");
        RemoteIP = string.IsNullOrEmpty(forward) ? Request.RemoteEndPoint.Address : IPAddress.Parse(forward.Split(",").First());

        var token = Request.Headers["Authorization"];
        token ??= Request.Cookies["token"]?.Value;

        if (string.IsNullOrEmpty(token))
        {
            AuthError = ErrorCodes.NoAuthorizationHeader;
            return;
        }

        if (!TokenCache.TryGet(token, out var id))
        {
            AuthError = ErrorCodes.InvalidToken;
            return;
        }

        var user = UserHelper.Get(id, false);

        if (user == null)
        {
            AuthError = ErrorCodes.UserNotFound;
            return;
        }

        UserID = user.ID;
        User = user;
    }

    #region Route Parameters

    private bool tryGetParameter(string name, out string value)
    {
        var success = Parameters.TryGetValue(name, out value!);

        if (!success)
            value = "";

        return success;
    }

    public bool TryGetStringParameter(string name, out string value)
    {
        if (tryGetParameter(name, out value))
            return true;

        ReplyError(ErrorCodes.MissingParameter).Wait();
        return false;
    }

    public bool TryGetLongParameter(string name, out long value)
    {
        if (tryGetParameter(name, out var str) && long.TryParse(str, out value))
            return true;

        ReplyError(ErrorCodes.InvalidParameter).Wait();
        value = 0;
        return false;
    }

    #endregion

    #region Query Parameters

    private bool tryGetQuery(string name, out string value)
    {
        var query = Request.QueryString.Get(name);

        if (!string.IsNullOrEmpty(query))
        {
            value = query;
            return true;
        }

        value = "";
        return false;
    }

    public bool TryGetStringQuery(string name, out string value)
    {
        if (tryGetQuery(name, out value))
            return true;

        ReplyError(ErrorCodes.MissingQuery).Wait();
        return false;
    }

    public string? GetStringQuery(string name)
        => tryGetQuery(name, out var value) ? value : null;

    public bool TryGetLongQuery(string name, out long value)
    {
        if (tryGetQuery(name, out var str) && long.TryParse(str, out value))
            return true;

        ReplyError(ErrorCodes.InvalidQuery).Wait();
        value = 0;
        return false;
    }

    public int? GetIntQuery(string name)
    {
        if (tryGetQuery(name, out var str) && int.TryParse(str, out var value))
            return value;

        return null;
    }

    #endregion

    #region Headers

    private bool tryGetHeader(string name, out string value)
    {
        var header = Request.Headers.Get(name);

        if (!string.IsNullOrEmpty(header))
        {
            value = header;
            return true;
        }

        value = "";
        return false;
    }

    public bool TryGetStringHeader(string name, out string value)
    {
        if (tryGetHeader(name, out value))
            return true;

        ReplyError(ErrorCodes.MissingHeader).Wait();
        return false;
    }

    #endregion

    #region Replying

    public async Task Reply(object? data = null) => await reply(new APIResponse
    {
        Code = ErrorCodes.None,
        Data = data
    });

    public async Task ReplyError(ErrorCodes code) => await reply(new APIResponse
    {
        Code = code
    });

    private async Task reply(APIResponse response)
    {
        var json = JsonConvert.SerializeObject(response);
        var buffer = Encoding.UTF8.GetBytes(json);
        await ReplyData(buffer, "application/json");
    }

    private bool replied;

    public async Task ReplyData(byte[] buffer, string type, string filename = "")
    {
        // worst case scenario i guess
        if (replied)
            return;

        replied = true;

        Response.ContentLength64 = buffer.Length;
        Response.ContentEncoding = Encoding.UTF8;
        Response.AddHeader("Content-Type", type);
        Response.AddHeader("Access-Control-Allow-Origin", Request.Headers.Get("Origin") ?? "*");
        Response.AddHeader("Access-Control-Allow-Methods", string.Join(", ", allowed_methods));
        Response.AddHeader("Access-Control-Allow-Headers", string.Join(", ", allowed_headers));

        if (!string.IsNullOrEmpty(filename))
            Response.AddHeader("Content-Disposition", $"attachment; filename=\"{filename}\"");

        try
        {
            await Response.OutputStream.WriteAsync(buffer);
        }
        catch (Exception e)
        {
            Logger.Log(e.Message, LogLevel.Error);
        }

        Response.Close();
        stopTimer();
    }

    #endregion

    #region Timing

    private Stopwatch timer { get; } = new();
    private bool timed;

    public void StartTimer()
    {
        timer.Start();
        timed = true;
    }

    private void stopTimer()
    {
        if (!timed)
            return;

        timer.Stop();
        Logger.Log($"Request took {timer.ElapsedMilliseconds}ms", LogLevel.Debug);
    }

    #endregion
}
