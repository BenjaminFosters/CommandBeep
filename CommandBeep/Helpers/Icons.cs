using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CommandBeep.Helpers;

internal static class Icons
{
    // References:https://learn.microsoft.com/en-us/windows/apps/design/iconography/segoe-fluent-icons-font
    // As far as I can tell, some MDL2 and Segoe Fluent Icons share same unicode. Thanks MSFT.

    public static IconInfo Chats { get; } = new("\ue8f2");
    public static IconInfo Open { get; } = new("\ue8a7");
    public static IconInfo Write { get; } = new("\uf67b");
    public static IconInfo Send { get; } = new("\ue724");
    public static IconInfo CBIcon { get; } = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
}
