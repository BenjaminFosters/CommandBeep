using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CommandBeep.Helpers;

internal static class Icons
{
    // References:https://learn.microsoft.com/en-us/windows/apps/design/iconography/segoe-fluent-icons-font
    // As far as I can tell, some MDL2 and Segoe Fluent Icons share same unicode. Thanks MSFT.

    public static IconInfo SingleChat { get; } = new("\ue716");
    public static IconInfo GroupChat { get; } = new("\ue902");
    public static IconInfo Open { get; } = new("\ue8a7");
    public static IconInfo Write { get; } = new("\uf67b");
    public static IconInfo Send { get; } = new("\ue724");
    public static IconInfo Offline { get; } = new("\uf384");
    public static IconInfo Reload { get; } = new("\ue72c");
    public static IconInfo Denied { get; } = new("\ueb90");
    public static IconInfo Bug { get; } = new("\uebe8");
    public static IconInfo CBIcon { get; } = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
}
