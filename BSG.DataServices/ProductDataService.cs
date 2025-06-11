using BSG.Common.DTO;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface IProductDataService : IDataServiceBase<ProductDto>
{
}

public class ProductDataService
    : DataServiceBase<ProductDto>, IProductDataService
{
    public ProductDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/product";
    }
}