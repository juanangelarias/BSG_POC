using System.Net.Http.Json;
using BSG.Common.DTO;
using BSG.Common.Exceptions;
using BSG.Common.Model;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface INotificationDataService: IDataServiceBase<NotificationDto>
{
}

public class NotificationDataService: DataServiceBase<NotificationDto>, INotificationDataService
{
    public NotificationDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/notification";
    }
    
    public async Task<List<NotificationDto>> GetByEmail(string email)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/GetByEmail/{email}");
        var response = await GetResponse(request);

        if (response == null)
            return [];
        
        var result = await response.Content.ReadFromJsonAsync<Response<List<NotificationDto>>>();
        
        if(result is {Success: true})
            return result.Content ?? [];
        
        if(result?.Error?.Code == "400")
            throw new NotFoundException(result.Error.Message);
        
        throw new DataServiceException("An error has occurred please retry later");
    }
}