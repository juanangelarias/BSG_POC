using BSG.Common.DTO;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface IProductDataService: IDataServiceBase<ProductDto>
{
}

public class ProductDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler) 
    : DataServiceBase<ProductDto>(client, state, errorHandler), IProductDataService
{
}