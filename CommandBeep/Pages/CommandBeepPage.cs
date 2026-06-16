// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommandBeep.Backend;
using CommandBeep.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CommandBeep;

internal sealed partial class CommandBeepPage : DynamicListPage
{
    private readonly BeeperSrv _beeperSrv;
    private string _query = string.Empty;
    private List<chatsByTitle> _chats = new();

    public CommandBeepPage()
    {
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
        _beeperSrv = new BeeperSrv("http://localhost:23373/", "bdapi_6mxSA1itq8ntUXKE1FvhpVkNOAuOvK3fWR8lKkTo8_w");
        LoadChatAsync();
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
                Subtitle = $"Over at {chat.network}",
                //Icon = chat.imgURL != null ? IconHelpers.FromRelativePath(new Uri(chat.imgURL).LocalPath) : IconHelpers.FromRelativePath("Assets\\StoreLogo.png"), (TO BE IMPLEMENTED)
                Command = new CommandBeepSendPage(_beeperSrv, chat.id, chat.title),
            }).ToArray();
        }
    }
}