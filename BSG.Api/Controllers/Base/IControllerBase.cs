using BSG.Common.DTO.Base;
using BSG.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace BSG.Api.Controllers.Base;

public interface IControllerBase<T>
    where T : class, IDtoBase
{
    [HttpGet]
    Task<ActionResult<List<T>>> GetAll();
    
    [HttpGet("GetPage")]
    Task<ActionResult<Response<PagedResponse<T>>>> GetPage([FromQuery] QueryParams parameters);
    
    [HttpGet("{id:long}")]
    Task<ActionResult<Response<T>>> GetById([FromRoute] long id);
    
    [HttpPost]
    Task<ActionResult<Response<T>>> Create([FromBody] T dto);
    
    [HttpPut]
    Task<ActionResult<Response<T>>> Update([FromBody] T dto);
    
    [HttpDelete("{id:long}")]
    Task<ActionResult<Response<bool>>> Delete([FromRoute] long id);
}