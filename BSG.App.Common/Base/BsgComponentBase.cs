using System.Collections.Specialized;
using BSG.App.Common.ErrorHandling;
using BSG.Common.DTO;
using BSG.Common.Model;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BSG.App.Common.Base;

public abstract class BsgComponentBase : ComponentBase, IDisposable
{
    [Inject] public required IExceptionRecorderService ExceptionRecorder { get; set; }
    [Inject] public required NotificationService Notification { get; set; }
    [Inject] public required TooltipService Tooltip { get; set; }

    protected string _component = "";
    protected List<Metadata>? _metadata;
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

    protected Metadata GetMetadata(string code)
    {
        return _metadata?.FirstOrDefault(f =>
                   string.Equals(f.Code, code, StringComparison.CurrentCultureIgnoreCase))
               ?? new Metadata
               {
                   Code = code,
                   Component = new ComponentDto { Id = 0, Name = _component },
                   DisplayName = "No title",
                   Help = "No help",
                   Tooltip = "",
                   IsEnabled = false,
                   IsVisible = true
               };
    }

    protected void ShowTooltip(ElementReference reference, string tooltip, TooltipOptions? options = null)
    {
        var opts = options ??
                   new TooltipOptions
                   {
                       Delay = 500,
                       Position = TooltipPosition.Bottom
                   };

        Tooltip.Open(reference, tooltip, opts);
    }

    protected Metadata GetBlankMetadata()
    {
        return new Metadata
        {
            Code = "NoCode",
            Component = new ComponentDto { Id = 0, Name = _component },
            DisplayName = "No title",
            Help = "No help",
            Tooltip = "",
            IsEnabled = false,
            IsVisible = true
        };
    }
}