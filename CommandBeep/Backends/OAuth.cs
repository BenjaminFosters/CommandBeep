using CommandBeep.Helpers;

using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Duende.IdentityModel.Client;

namespace CommandBeep.Backends;

internal class OAuth
{
    private readonly SettingsManager _settingsManager;
    private readonly HttpClient _httpClient = new();

    public OAuth(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _httpClient.BaseAddress = new Uri(_settingsManager.Endpoint);
    }

    public async Task GetOAuthToken() // Some of the part are co-authored by A\'s Claude Sonnet 5
    {
        var verifier = HelperGet.CodeVerifier();
        var challenge = HelperGet.CodeChallenge(verifier);
        var state = HelperGet.State();
        var port = HelperGet.FreePort();
        var redirectUri = $"http://127.0.0.1:{port}/callback/";

        using var listener = new HttpListener();
        listener.Prefixes.Add(redirectUri);
        listener.Start();

        var authUrl =
            $"{new Uri(_httpClient.BaseAddress!, "oauth/authorize")}" +
            $"?response_type=code" +
            $"&client_id={Uri.EscapeDataString("CommandBeep")}" +
            $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
            $"&code_challenge={Uri.EscapeDataString(challenge)}" +
            $"&code_challenge_method=S256" +
            $"&state={Uri.EscapeDataString(state)}" +
            $"&scope={Uri.EscapeDataString("read write")}";

        Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });

        var context = await listener.GetContextAsync();
        var query = context.Request.QueryString;
        var returnedState = query["state"];
        var code = query["code"];

        var html = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Backends", "oauth.html"));

        var responseBytes = Encoding.UTF8.GetBytes(html);
        context.Response.ContentType = "text/html";
        context.Response.ContentLength64 = responseBytes.Length;
        await context.Response.OutputStream.WriteAsync(responseBytes);
        context.Response.OutputStream.Close();
        listener.Stop();

        if (returnedState != state)
            throw new InvalidOperationException("OAuth state mismatch.");

        if (string.IsNullOrEmpty(code))
            throw new InvalidOperationException("No authorization code returned.");

        var tokenResponse = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
        {
            Address = new Uri(_httpClient.BaseAddress!, "oauth/token").ToString(),
            ClientId = "CommandBeep",
            Code = code,
            RedirectUri = redirectUri,
            CodeVerifier = verifier,
            ClientCredentialStyle = ClientCredentialStyle.PostBody
        });

        if (tokenResponse.IsError)
            throw new InvalidOperationException($"Token exchange failed: {tokenResponse.Error} - {tokenResponse.ErrorDescription}");

        _settingsManager.SetAPIKey(tokenResponse.AccessToken);
    }

}

internal static class HelperGet
{
    public static string CodeVerifier()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Base64UrlEncode(bytes);
    }

    public static string CodeChallenge(string verifier)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(verifier));
        return Base64UrlEncode(bytes);
    }

    public static string FreePort()
    {
        var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        var port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port.ToString();
    }

    public static string State() => Guid.NewGuid().ToString("N");

    private static string Base64UrlEncode(byte[] bytes) => Base64Url.EncodeToString(bytes);
}