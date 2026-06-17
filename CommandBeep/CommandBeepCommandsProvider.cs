// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CommandBeep;

public partial class CommandBeepCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public CommandBeepCommandsProvider()
    {
        DisplayName = "CommandBeep";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        _commands = [
            new CommandItem(new CommandBeepPage()) { Title = DisplayName, Subtitle = "Send your Beeper Messages through Command Palette Window" },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
