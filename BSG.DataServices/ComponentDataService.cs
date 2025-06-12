using System.Net.Http.Json;
using BSG.Common.DTO;
using BSG.Common.Exceptions;
using BSG.Common.Model;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface IComponentDataService : IDataServiceBase<ComponentDto>
{
    Task<List<ComponentDto>> GetExtended(); 
}

public class ComponentDataService
    : DataServiceBase<ComponentDto>, IComponentDataService
{
    public ComponentDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/component";
    }
    
    public async Task<List<ComponentDto>> GetExtended()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/getExtended");
        var response = await GetResponse(request);
        if (response == null)
            return [];

        var result = await response.Content.ReadFromJsonAsync<Response<List<ComponentDto>>>();

        if (result is { Success: true })
            return result.Content ?? [];

        if (result?.Error?.Code =="400")
            throw new NotFoundException(result.Error.Message);

        throw new DataServiceException("An error has occurred please retry later");
    }
}