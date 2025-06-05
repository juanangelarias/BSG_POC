using BSG.Common.DTO;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface IProductTypeDataService: IDataServiceBase<ProductTypeDto>
{
}

public class ProductTypeDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler) 
    : DataServiceBase<ProductTypeDto>(client, state, errorHandler), IProductTypeDataService
{
}