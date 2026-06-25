using Microsoft.CommandPalette.Extensions.Toolkit;
using System.IO;

namespace CommandBeep.Helpers;

public sealed class SettingsManager : JsonSettingsManager
{
    private readonly TextSetting _endpoint = new("endpoint", "Beeper Desktop API Endpoint", "Beeper Desktop Endpoint URL", "http://127.0.0.1:23373") { Placeholder = "For OAuth 2.0, use 127.0.0.1 instead of localhost" };
    private readonly TextSetting _apiKey = new("apiKey", "Beeper Desktop API Key", "Beeper Desktop API Key", "") { Placeholder = "bdapi_4e65766572476f6E6e6147697665596f755570_w" };

    public string Endpoint => _endpoint.Value ?? "http://127.0.0.1/";
    public string ApiKey => _apiKey.Value ?? string.Empty;

    public SettingsManager()
    {
        this.FilePath = SettingPath();
        this.Settings.Add(this._endpoint);
        this.Settings.Add(this._apiKey);
        this.LoadSettings();
        this.Settings.SettingsChanged += (_, _) => this.SaveSettings();
    }

    private static string SettingPath()
    {
        var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "settings.json");
    }
}