// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommandBeep.Backends;
using CommandBeep.Pages;
using CommandBeep.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace CommandBeep;

internal sealed partial class CommandBeepPage : DynamicListPage
{
    private BeeperSrv _beeperSrv;
    private string _query = string.Empty;
    private List<chatsByTitle> _chats = new();
    private readonly SettingsManager _settingsManager;
    

    public CommandBeepPage(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;

        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "CommandBeep";
        Name = "Start Sending Messages";
        PlaceholderText = "Type out Chat/Contact name (e.g. John Doe)";
        EmptyContent = new CommandItem()
        {
            Title = "No Chats to Show",
            Subtitle = "Start by typing the Chat/Contact name (e.g. John Doe)",
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png"),
        };

        buildBeeperSrv();
        _settingsManager.Settings.SettingsChanged += (_, _) => buildBeeperSrv();
    }

    public void buildBeeperSrv()
    {
        _beeperSrv = new BeeperSrv(_settingsManager.Endpoint, _settingsManager.ApiKey);
    }
    
    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        _query = newSearch;
        _ = LoadChatAsync();
    }

    private async Task LoadChatAsync()
    {
        _chats = await _beeperSrv.fetchChatList(_query);
        RaiseItemsChanged();
    }

    public override IListItem[] GetItems()
    {
        if (_chats.Count == 0)
        {
            return [];
        } else
        {
            return _chats.Select(chat => new ListItem()
            {
                Title = chat.title,
                Subtitle = $"@ {chat.network}",
                Icon = new IconInfo("\ue8f2"),
                Command = new CommandBeepSendPage(_beeperSrv, chat.id, chat.title),
            }).ToArray();
        }
    }
}