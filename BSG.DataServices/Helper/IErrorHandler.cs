using System.Collections.ObjectModel;

namespace BSG.DataServices.Helper;

public interface IErrorHandler
{
    ObservableCollection<Exception> Exceptions { get; set; }
}