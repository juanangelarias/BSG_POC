using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BSG.App.Common.Components;

public class BSGComponentBase: ComponentBase
{
    [Inject] public required ThemeService ThemeService { get; set; }
    [Inject] public required DialogService DialogService { get; set; }
    [Inject] public required NotificationService NotificationService { get; set; }

    #region Fields & Properties

    public bool IsLoading { get; set; }

    #endregion
    
    protected async Task ShowLoading()
    {
        IsLoading = true;

        await Task.Yield();

        IsLoading = false;
    }
    
    public async Task BusyDialog(string message)
    {
        await DialogService.OpenAsync<BusyDialog>("",
            new Dictionary<string, object> { { "Message", message } },
            new DialogOptions
            {
                Resizable = false,
                Draggable = false,
                CloseDialogOnEsc = false,
                CloseDialogOnOverlayClick = false,
                ShowClose = false,
                ShowTitle = false,
                Style = "min-height:auto;min-width:auto;width:auto"
            });
    }
    
    protected void NotifySuccessfulSave(string itemSaved = "Changes")
    {
        itemSaved = itemSaved[..1].ToUpper() + itemSaved[1..];

        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Info,
            Summary = $"{itemSaved} saved successfully.",
            Duration = 5000
        });
    }
    
    protected void NotifySaveError(string[] errors, string itemSaved = "changes")
    {
        var summary = $"There {(errors.Length == 1 ? "was an error" : "were errors")} " +
                      $"saving {itemSaved}.  {string.Join("  ", errors)}";

        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Error,
            Summary = summary,
            Duration = 300000 // Treat as fatal 5 minutes
        });
    }
    
    protected void NotifyLoadError(string[] errors, string itemSaved = "data", bool fatal = false)
    {
        var msg = fatal
            ? $"There {(errors.Length == 1 ? "was a fatal error" : "were fatal error(s)"
                )} retrieving {itemSaved}.  {string.Join("  ", errors)}"
            : $"There was an error retrieving {itemSaved}.  Retrying...";

        NotificationService.Notify(new NotificationMessage
        {
            Severity = fatal ? NotificationSeverity.Error : NotificationSeverity.Warning,
            Summary = msg,
            Duration = fatal ? 300000 : 15000 // Fatal: 5 minutes Else: 15 seconds
        });
    }
    
    protected void NotifyError(string[] errors, string message = "There was an error.")
    {
        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Error,
            Summary = message,
            Duration = 5000
        });
    }
}