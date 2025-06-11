using System.Collections.Specialized;
using BSG.App.Common.ErrorHandling;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BSG.App.Common.Base;

public abstract class BsgComponentBase : ComponentBase, IDisposable
{
    [Inject] public required IExceptionRecorderService ExceptionRecorder { get; set; }
    [Inject] public required NotificationService Notification { get; set; }
    [Inject] public required TooltipService Tooltip { get; set; }

    public long ComponentId { get; set; } = 1;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        ExceptionRecorder.Exceptions.CollectionChanged += RefreshUi;
    }

    public void RefreshUi(object? sender, NotifyCollectionChangedEventArgs eventArgs)
    {
        foreach (var exception in ExceptionRecorder.Exceptions)
        {
            Notification.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = exception.Message,
                Duration = 4000
            });
        }
    }

    public void Dispose()
    {
        ExceptionRecorder.Exceptions.CollectionChanged -= RefreshUi;
    }

    protected string GetTitle(string code)
    {
        if (ComponentId == 1)
            return code.ToLower() switch
            {
                "btncreate" => "Add New",
                "colusername" => "Username",
                "colfullname" => "Full Name",
                "colisadmin" => "Is Admin?",
                "btnedit" => "Edit",
                "btndelete" => "Delete",
                _ => "No Title"
            };

        return "No Title";
    }

    protected string GetTooltip(string code, TooltipOptions? options = null)
    {
        if (ComponentId == 1)
            return code.ToLower() switch
            {
                "btncreate" => "Create a new user",
                "colusername" => "Write the Username",
                "colfullname" => "Write the full name of the user",
                "colisadmin" => "Check if the user is an administrator",
                "btnedit" => "Edit the user data",
                "btndelete" => "Delete the user",
                _ => ""
            };

        return "";
    }
    
    protected void ShowTooltip(ElementReference reference, string code, TooltipOptions? options = null)
    {
        var text = "";
        if (ComponentId == 1)
            text = code.ToLower() switch
            {
                "btncreate" => "Create a new user",
                "colusername" => "Write the Username",
                "colfullname" => "Write the full name of the user",
                "colisadmin" => "Check if the user is an administrator",
                "btnedit" => "Edit the user data",
                "btndelete" => "Delete the user",
                _ => ""
            };
        
        Tooltip.Open(reference, text, options);
    }
}