using System.Collections.Specialized;
using BSG.App.Common.ErrorHandling;
using BSG.Common.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BSG.App.Common.Base;

public abstract class BsgComponentBase : ComponentBase, IDisposable
{
    [Inject] public required IExceptionRecorderService ExceptionRecorder { get; set; }
    [Inject] public required NotificationService Notification { get; set; }
    [Inject] public required TooltipService Tooltip { get; set; }

    protected string _component = "";
    protected ComponentDto? _metadata;
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
        return _metadata!.Elements
            .FirstOrDefault(f => f.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase))?
            .DisplayName ?? "No Title";

    }

    protected string GetTooltip(string code)
    {
        return _metadata!.Elements
            .FirstOrDefault(f => f.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase))?
            .Tooltip ?? "";
    }
    
    protected string GetHelp(string code)
    {
        return _metadata!.Elements
            .FirstOrDefault(f => f.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase))?
            .Help ?? "";
    }
    
    protected void ShowTooltip(ElementReference reference, string code, TooltipOptions? options = null)
    {
        var opts = options ??
                   new TooltipOptions
                   {
                       Delay = 500,
                       Duration = 1000,
                       Position = TooltipPosition.Bottom
                   };

        var text = GetTooltip(code);
        
        Tooltip.Open(reference, text, opts);
    }
}