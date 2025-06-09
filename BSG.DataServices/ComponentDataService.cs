using BSG.Common.DTO;
using BSG.DataServices.Base;
using BSG.DataServices.Helper;
using BSG.States;

namespace BSG.DataServices;

public interface IComponentDataService : IDataServiceBase<ComponentDto>
{
}

public class ComponentDataService 
    : DataServiceBase<ComponentDto>, IComponentDataService
{
    public ComponentDataService(HttpClient client, IGeneralState state, IErrorHandler errorHandler)
    : base(client, state, errorHandler)
    {
        BaseUrl = "api/component";
    }
}