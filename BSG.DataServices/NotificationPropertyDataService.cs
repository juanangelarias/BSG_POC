using BSG.Common.DTO;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface INotificationPropertyDataService: IDataServiceBase<NotificationPropertyDefinitionDto>
{
}

public class NotificationPropertyDataService: DataServiceBase<NotificationPropertyDefinitionDto>, INotificationPropertyDataService
{
    public NotificationPropertyDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/notificationProperty";
    }
}