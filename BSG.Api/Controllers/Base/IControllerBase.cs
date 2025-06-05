using BSG.Common.DTO.Base;
using BSG.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace BSG.Api.Controllers.Base;

public interface IControllerBase<T>
    where T : class, IDtoBase
{
    Task<ActionResult<List<T>>> GetAll();
    Task<ActionResult<Response<PagedResponse<T>>>> GetPage([FromQuery] QueryParams parameters);
    Task<ActionResult<Response<T>>> GetById([FromRoute] long id);
    Task<ActionResult<Response<T>>> Create([FromBody] T dto);
    Task<ActionResult<Response<T>>> Update([FromBody] T dto);
    Task<ActionResult<Response<bool>>> Delete([FromRoute] long id);
}