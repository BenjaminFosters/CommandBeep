using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using CommandBeep.Backends;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CommandBeep.Pages;

internal partial class CommandBeepSendPage : DynamicListPage {
    private readonly BeeperSrv _beeperSrv;
    string _chatTitle = string.Empty;
    string _chatId = string.Empty;
    string _query = string.Empty;
    public CommandBeepSendPage(BeeperSrv beeperSrv, string chatId, string chatTitle)
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = $"CommandBeep - Sending to {chatTitle}";
        Name = "Send a Message";
        PlaceholderText = "Type in your message (such as helloing your recepient)";
        EmptyContent = new CommandItem()
        {
            Title = "We're Waiting For Your Message",
            Subtitle = "Just a \"Hello, World!\" will do alright?",
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png"),
        };
        _beeperSrv = beeperSrv;
        _chatId = chatId;
        _chatTitle = chatTitle;
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        _query = newSearch;
        RaiseItemsChanged();
    }

    public override IListItem[] GetItems()
    {
        if (_query == string.Empty)
        {
            return [new ListItem() {
                Title = "Open in Beeper",
                Subtitle = "with your chat ready!",
                Icon = new IconInfo("\ue8a7"),
                Command = new AnonymousCommand(() => {
                    _ = _beeperSrv.focusMessage(_chatId, _query);
                }) { Name = "Open", Result = CommandResult.ShowToast("Opened in Beeper") }
            },
            new ListItem() {
                Title = "Or type in your message to quickly send it.",
                Subtitle = "Go ahead, even a \"Hello, World!\" will do!",
                Icon = new IconInfo("\uf67b"),
            }];
        }
        else
        {
            return [new ListItem()
            {
                Title = $"Send \"{_query}\"",
                Subtitle = $"to {_chatTitle}",
                Icon = new IconInfo("\ue724"),
                Command = new AnonymousCommand(() => {
                    _ = _beeperSrv.sendMessage(_chatId, _query);
                }) { Name = "Send", Result = CommandResult.ShowToast("Message Sent") }
            },
            new ListItem() {
                Title = "Open in Beeper instead",
                Subtitle = "and bring your draft to Beeper's message composer.",
                Icon = new IconInfo("\ue8a7"),
                Command = new AnonymousCommand(() => {
                    _ = _beeperSrv.focusMessage(_chatId, _query); 
                }) { Name = "Open", Result = CommandResult.ShowToast("Opened in Beeper") }
            }];
        }
    }
}