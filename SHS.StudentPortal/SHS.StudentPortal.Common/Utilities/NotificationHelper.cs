using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace SHS.StudentPortal.Common.Utilities;

public static class NotificationHelper
{
    public static NotificationMessage NewNotif(string message, NotificationSeverity severity,
        string? summary = null, int duration = 3000, Action? onClose = null)
    {
        var notif = new NotificationMessage
        {
            Detail = message.Trim(),
            Severity = severity,
            Summary = summary is null ? severity.GetDisplayDescription() : summary,
            Duration = duration,
            ShowProgress = true,
        };

        if(onClose is not null)
            notif.Close = new Action<NotificationMessage>(action => onClose.Invoke());

        return notif;
    }

    public static NotificationMessage SuccessNotif(string message, string? summary = null, int duration = 3000, Action? onClose = null)
    {
        return NewNotif(message, NotificationSeverity.Success, summary, duration, onClose);
    }

    public static NotificationMessage InfoNotif(string message, string? summary = null, int duration = 3000, Action? onClose = null)
    {
        return NewNotif(message, NotificationSeverity.Info, summary, duration, onClose);
    }

    public static NotificationMessage WarningNotif(string message, string? summary = null, int duration = 3000, Action? onClose = null)
    {
        return NewNotif(message, NotificationSeverity.Warning, summary, duration, onClose);
    }

    public static NotificationMessage ErrorNotif(string message, string? summary = null, int duration = 3000, Action? onClose = null)
    {
        return NewNotif(message, NotificationSeverity.Error, summary, duration, onClose);
    }
}
