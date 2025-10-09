using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BSG.Common.DTO;
using BSG.Common.Exceptions;
using BSG.Common.Model;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;
using Constants = BSG.States.Model.FrontEndConstants;

namespace BSG.DataServices;

public interface IProductDataService : IDataServiceBase<ProductDto>
{
    Task<List<ProductDto>> GetExtended();
    Task<bool> CreateMany(List<ProductDto> products);
    Task<bool> UpdateMany(List<ProductDto> products);
    Task<List<ProductDto>> Search(string filter);
}

public class ProductDataService
    : DataServiceBase<ProductDto>, IProductDataService
{
    public ProductDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/product";
    }

    public async Task<List<ProductDto>> GetExtended()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/GetExtended");
        var response = await GetResponse(request);

        if (response == null)
            return [];

        var result = await response.Content.ReadFromJsonAsync<Response<List<ProductDto>>>();

        if (result is { Success: true })
            return result.Content ?? [];

        if (result?.Error?.Code =="400")
            throw new NotFoundException(result.Error.Message);

        throw new DataServiceException("An error has occurred please retry later");
    }
    
    public async Task<List<ProductDto>> Search(string filter)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Search/{filter}");
        var response = await GetResponse(request);

        if (response == null)
            return [];

        var result = await response.Content.ReadFromJsonAsync<Response<List<ProductDto>>>();

        if (result is { Success: true })
            return result.Content ?? [];

        if (result?.Error?.Code =="400")
            throw new NotFoundException(result.Error.Message);

        throw new DataServiceException("An error has occurred please retry later");
    }

    public async Task<bool> CreateMany(List<ProductDto> products)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/createMany")
        {
            Content = new StringContent(JsonSerializer.Serialize(products), Encoding.UTF8, Constants.MediaType)
        };
        var response = await GetResponse(request);

        if (response == null)
            throw new DataServiceException("An error has occurred please retry later");

        var result = await response.Content.ReadFromJsonAsync<Response<List<ProductDto>>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        return result.Success;
    }

    public async Task<bool> UpdateMany(List<ProductDto> products)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/updateMany")
        {
            Content = new StringContent(JsonSerializer.Serialize(products), Encoding.UTF8, Constants.MediaType)
        };
        var response = await GetResponse(request);

        if (response == null)
            return false;

        var result = await response.Content.ReadFromJsonAsync<Response<List<ProductDto>>>();
        
        if(result == null)
            throw new DataServiceException("An error has occurred please retry later");

        return result?.Success ?? false;
    }
}