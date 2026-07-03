using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Threading.Tasks;

namespace CommandBeep.Helpers;

internal static class Shorthanders
{
    public static AnonymousCommand CommandWithToast(Func<Task> action, string label, string toastMessage)
    {
        return new AnonymousCommand(() => action()) { Name = label, Result = CommandResult.ShowToast(toastMessage) };
    }

    public static AnonymousCommand CommandShorthands(Func<Task> action, string label)
    {
        return new AnonymousCommand(() => action()) { Name = label };
    }

    public static AnonymousCommand CommandKeepOpen(Func<Task> action, string label)
    {
        return new AnonymousCommand(() => action()) { Name = label, Result = CommandResult.KeepOpen() };
    }
}