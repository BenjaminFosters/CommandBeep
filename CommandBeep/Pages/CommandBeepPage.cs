// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CommandBeep;

internal sealed partial class CommandBeepPage : ListPage
{
    public CommandBeepPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "CommandBeep";
        Name = "Open";
    }

    public override IListItem[] GetItems()
    {
        var command = new OpenUrlCommand("https://beeper.com");
        return [
            new ListItem(command) { Title = "Open Beeper"}
        ];
    }
}
