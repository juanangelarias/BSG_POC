using BSG.Common.Model;

namespace BSG.DataServices.Base;

public interface IDataServiceBase<T>
    where T : class
{
    string Token { get; set; }
    
    Task<List<T>> Get();
    Task<PagedResponse<T>> GetPage( QueryParams parameters );
    Task<T?> Get( long id );
    Task<T?> Create( T dto );
    Task<T?> Update( T dto );
    Task Delete( long id );
    Task<HttpResponseMessage?> GetResponse( HttpRequestMessage request );
}