// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

using System.Linq;
using System.Threading.Tasks;
using System.Net;

using CommandBeep.Backends;
using CommandBeep.Pages;
using CommandBeep.Helpers;
using static CommandBeep.Helpers.Shorthanders;

namespace CommandBeep;

internal sealed partial class CommandBeepPage : DynamicListPage
{
    private BeeperSrv _beeperSrv;
    private string _query = string.Empty;
    private FetchChatListResponse _response;
    private readonly SettingsManager _settingsManager;
    private readonly OAuth _oauth;

    public CommandBeepPage(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;

        Icon = Icons.CBIcon;
        Title = "CommandBeep";
        Name = "Start Sending Messages";
        PlaceholderText = "Type out Chat/Contact name (e.g. John Doe)";
        EmptyContent = new CommandItem()
        {
            Title = "No Chats to Show",
            Subtitle = "Start by typing the Chat/Contact name (e.g. John Doe)",
            Icon = Icons.CBIcon,
        };

        _oauth = new OAuth(_settingsManager);

        BuildBeeperSrv();
        _settingsManager.Settings.SettingsChanged += (_, _) => BuildBeeperSrv();
    }

    public void BuildBeeperSrv()
    {
        _beeperSrv = new BeeperSrv(_settingsManager.Endpoint, _settingsManager.ApiKey);
        _ = LoadChatAsync();
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        _query = newSearch;
        _ = LoadChatAsync();
    }

    private async Task LoadChatAsync()
    {
        _response = await _beeperSrv.fetchChatList(_query);
        RaiseItemsChanged();
    }

    public override IListItem[] GetItems()
    {
        switch (_response.StatusCode)
        {
            case HttpStatusCode.OK:
                return _response.Items.Where(chat => !chat.IsReadOnly).Select(chat => new ListItem()
                {
                    Title = chat.Title,
                    Subtitle = $"@ {chat.Network} ({(chat.Type == "single" ? "Direct Message" : "Group")})",
                    Icon = chat.Type == "single" ? Icons.SingleChat : Icons.GroupChat,
                    Command = new CommandBeepSendPage(_beeperSrv, chat.Id, chat.Title),
                    MoreCommands = [
                        new CommandContextItem(CommandWithToast(() => _beeperSrv.focusChat(chat.Id, ""), "Open in Beeper", "Opened in Beeper")) { Icon = Icons.Open },
                        new CommandContextItem(new CopyTextCommand(chat.Id) { Name = "Copy Chat ID"}),
                    ]
                }).ToArray();

            case HttpStatusCode.Unauthorized:
                return [new ListItem()
                {
                    Title = "Invalid API Key",
                    Subtitle = "Please Update It",
                    Icon = Icons.Denied
                },
                new ListItem()
                {
                    Title = "Connect your Beeper Desktop",
                    Subtitle = "Using OAuth 2.0 for Authorization",
                    Command = CommandShorthands(async () => { await _oauth.GetOAuthToken(); BuildBeeperSrv(); }, "Authorize"),
                    Icon = Icons.Auth
                },
                new ListItem()
                {
                    Title = "Update in Settings",
                    Subtitle = "Manually add your API Key (and also your endpoint)",
                    Command = _settingsManager.Settings.SettingsPage,
                },
                new ListItem()
                {
                    Title = "Reload Connection",
                    Subtitle = "cus after updating, you *might* need to turn things off and on back.",
                    Icon = Icons.Reload,
                    Command = CommandKeepOpen(() => { BuildBeeperSrv(); return Task.CompletedTask; }, "Reload")
                },];

            case HttpStatusCode.ServiceUnavailable:
                return [new ListItem()
                {
                    Title = $"Can't connect to {_settingsManager.Endpoint}",
                    Subtitle = "Make sure your Beeper is up and running.",
                    Icon = Icons.Offline,
                },
                new ListItem()
                {
                    Title = "Reload Connection (especially after cold boot)",
                    Subtitle = "Have you tried to turn it off and back on again?",
                    Icon = Icons.Reload,
                    Command = CommandKeepOpen(() => { BuildBeeperSrv(); return Task.CompletedTask; }, "Reload")
                },
                new ListItem()
                {
                    Title = "If your endpoint is wrong.",
                    Subtitle = "Then change it on settings.",
                    Command = _settingsManager.Settings.SettingsPage,
                }];

            case HttpStatusCode.NotFound:
                return [new ListItem()
                {
                    Title = $"API Access Disabled",
                    Subtitle = "Enable your Desktop API access.",
                    Icon = Icons.Denied,
                },
                new ListItem()
                {
                    Title = "Reload Connection",
                    Subtitle = "Have you tried to turn it off and back on again?",
                    Icon = Icons.Reload,
                    Command = CommandKeepOpen(() => { BuildBeeperSrv(); return Task.CompletedTask; }, "Reload")
                },
                new ListItem()
                {
                    Title = "If your endpoint is wrong.",
                    Subtitle = "Then change it on settings.",
                    Command = _settingsManager.Settings.SettingsPage,
                }];

            default:
                return [new ListItem() {
                    Title = "Pardon me! We haven't seen this error before!",
                    Subtitle = $"Error: {_response.StatusCode}",
                    Icon = Icons.Bug,
                },
                //new ListItem() {
                //    Title = "Copy Error Message"
                //}
                ];
        }
    }
}