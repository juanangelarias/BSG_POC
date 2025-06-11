using System.Net.Http.Json;
using BSG.Common.DTO;
using BSG.Common.Exceptions;
using BSG.Common.Model;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface IElementDataService : IDataServiceBase<ElementDto>
{
    Task<List<ElementDto>> GetByComponentId(long compoentId);
}

public class ElementDataService
    : DataServiceBase<ElementDto>, IElementDataService
{
    public ElementDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/Element";
    }

    public async Task<List<ElementDto>> GetByComponentId(long compoentId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/getByComponentId/{compoentId}");

        var response = await GetResponse(request);

        if (response == null)
            return [];

        var result = await response.Content.ReadFromJsonAsync<Response<List<ElementDto>>>();

        if (result == null)
            throw new DataServiceException("An error has occured, please try again later");

        return !result.Success
            ? throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later")
            : result.Content ?? [];
    }
}