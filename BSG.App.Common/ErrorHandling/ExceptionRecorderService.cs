using System.Collections.ObjectModel;

namespace BSG.App.Common.ErrorHandling;

public interface IExceptionRecorderService
{
    ObservableCollection<Exception> Exceptions { get; set; }
}

public class ExceptionRecorderService: IExceptionRecorderService
{
    public ObservableCollection<Exception> Exceptions { get; set; } = [];
}