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
}
