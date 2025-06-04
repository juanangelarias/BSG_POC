using System.Collections.ObjectModel;

namespace BSG.DataServices.Helper;

public class ErrorHandler: IErrorHandler
{
    public ObservableCollection<Exception> Exceptions { get; set; } = [];
}