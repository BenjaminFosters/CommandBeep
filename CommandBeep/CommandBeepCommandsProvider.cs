// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

using CommandBeep.Helpers;

namespace CommandBeep;

public partial class CommandBeepCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly SettingsManager _settingsManager = new();

    public CommandBeepCommandsProvider()
    {
        Id = "id.my.reubenhu.commandbeep";
        DisplayName = "CommandBeep";
        Settings = _settingsManager.Settings;

        Icon = Icons.CBIcon;
        _commands = [
            new CommandItem(new CommandBeepPage(_settingsManager)) {
                Title = DisplayName,
                Subtitle = "Send your Beeper Messages through Command Palette Window",
                MoreCommands = [new CommandContextItem(_settingsManager.Settings.SettingsPage)]
            },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
