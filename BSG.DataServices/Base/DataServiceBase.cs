using System.Net.Http.Json;
using System.Text;
using BSG.Common.DTO.Base;
using BSG.Common.Exceptions;
using BSG.Common.Model;
using BSG.DataServices.Helper;
using BSG.States;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Constants = BSG.States.Model.FrontEndConstants;

namespace BSG.DataServices.Base;

public class DataServiceBase<T>(
    HttpClient client,
    IGeneralState state,
    IErrorHandler errorHandler)
    : IDataServiceBase<T>
    where T : DtoBase
{
    public string Token { get; set; } = state.Token ?? "";

    protected string BaseUrl { get; init; } = string.Empty;

    public async Task<List<T>> Get()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
        var response = await GetResponse(request);

        if (response == null)
            return [];

        var result = await response.Content.ReadFromJsonAsync<Response<List<T>>>();

        if (result is { Success: true })
            return result.Content ?? [];

        if (result?.Error?.Code =="400")
            throw new NotFoundException(result.Error.Message);

        throw new DataServiceException("An error has occurred please retry later");
    }

    public async Task<PagedResponse<T>> GetPage(QueryParams parameters)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/getPage" +
                                                             $"?filter={parameters.Filter}" +
                                                             $"&PageSize={parameters.PageSize}" +
                                                             $"&PageIndex={parameters.PageIndex}" +
                                                             $"&Sort={parameters.Sort}" +
                                                             $"&Descending={parameters.Descending}" +
                                                             $"&Expand={parameters.Expand}");
        var response = await GetResponse(request);

        if (response == null)
            throw new DataServiceException("An error has occurred please retry later");

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Response<PagedResponse<T>>>(json);
        if (result == null)
            throw new DataServiceException("An error has occurred please retry later");

        return !result.Success
            ? throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later")
            : result.Content ?? new PagedResponse<T>([], 0);
    }

    public async Task<T?> Get(long id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{id}");
        var response = await GetResponse(request);

        if (response == null)
            throw new DataServiceException("An error has occurred please retry later");

        var result = await response.Content.ReadFromJsonAsync<Response<T>>();

        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        return !result.Success 
            ? throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later") 
            : result.Content;
    }

    public async Task<T?> Create(T dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, Constants.MediaType)
        };
        var response = await GetResponse(request);

        if (response == null)
            throw new DataServiceException("An error has occurred please retry later");

        var result = await response.Content.ReadFromJsonAsync<Response<T>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        return !result.Success 
            ? throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later") 
            : result.Content;
    }

    public async Task<T?> Update(T dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, BaseUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, Constants.MediaType)
        };
        var response = await GetResponse(request);

        if (response == null)
            return null;

        var result = await response.Content.ReadFromJsonAsync<Response<T>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        return !result.Success ? 
            throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later") 
            : result.Content;
    }

    public async Task Delete(long id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/{id}");

        var response = await GetResponse(request);
        
        var result = await response!.Content.ReadFromJsonAsync<Response<string>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        if (!result.Success)
            throw new DataServiceException(result.Error?.Message ?? "An error has occurred please retry later");
    }

    public async Task<HttpResponseMessage?> GetResponse(HttpRequestMessage request)
    {
        /*if (!string.IsNullOrEmpty(Token))
        {
            request.Headers.Add("Authorization", $"Bearer {Token}");
        }*/

        HttpResponseMessage? response = null;

        try
        {
            response = await client.SendAsync(request);
        }
        catch (Exception exception)
        {
            errorHandler.Exceptions.Add(exception);
        }

        return response;
    }
}