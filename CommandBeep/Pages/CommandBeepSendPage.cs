using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

using CommandBeep.Backends;
using CommandBeep.Helpers;
using static CommandBeep.Helpers.Shorthanders;

namespace CommandBeep.Pages;

internal partial class CommandBeepSendPage : DynamicListPage
{
    private readonly BeeperSrv _beeperSrv;
    string _chatTitle = string.Empty;
    string _chatId = string.Empty;
    string _query = string.Empty;
    public CommandBeepSendPage(BeeperSrv beeperSrv, string chatId, string chatTitle)
    {
        Icon = Icons.Send;
        Title = $"CommandBeep - Sending to {chatTitle}";
        Name = "Send a Message";
        PlaceholderText = "Type in your message (such as helloing your recepient)";
        EmptyContent = new CommandItem()
        {
            Title = "We're Waiting For Your Message",
            Subtitle = "Just a \"Hello, World!\" will do alright?",
            Icon = Icons.CBIcon,
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
        if (string.IsNullOrEmpty(_query))
        {
            return [new ListItem() {
                Title = "Open in Beeper",
                Subtitle = "with your chat ready!",
                Icon = Icons.Open,
                Command = CommandWithToast(() => _beeperSrv.focusChat(_chatId, _query), "Open", "Opened in Beeper")
            },
            new ListItem() {
                Title = "Or type in your message to quickly send it.",
                Subtitle = "Go ahead, even a \"Hello, World!\" will do!",
                Icon = Icons.Write,
            }];
        }
        else
        {
            return [new ListItem()
            {
                Title = $"Send \"{_query}\"",
                Subtitle = $"to {_chatTitle}",
                Icon = Icons.Send,
                Command = CommandWithToast(() => _beeperSrv.sendMessage(_chatId, _query), "Send", "Message Sent")
            },
            new ListItem() {
                Title = "Open in Beeper instead",
                Subtitle = "and bring your draft to Beeper's message composer.",
                Icon = Icons.Open,
                Command = CommandWithToast(() => _beeperSrv.focusChat(_chatId, _query), "Open", "Opened in Beeper")
            }];
        }
    }
}