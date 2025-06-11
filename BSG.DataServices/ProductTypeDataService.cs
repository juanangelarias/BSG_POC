using BSG.Common.DTO;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface IProductTypeDataService: IDataServiceBase<ProductTypeDto>
{
}

public class ProductTypeDataService 
    : DataServiceBase<ProductTypeDto>, IProductTypeDataService
{
    public ProductTypeDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
        : base(client, state, errorHandler)
    {
        BaseUrl = "api/productType";
    }
}