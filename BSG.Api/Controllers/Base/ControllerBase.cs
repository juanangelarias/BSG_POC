using BSG.Common.DTO.Base;
using BSG.Common.Model;
using BSG.Entities.Base;
using BSG.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BSG.Api.Controllers.Base;

[ApiController]
public class ControllerBase<TEntity, TDto>(
    IWebHostEnvironment environment,
    IRepositoryExtended<TEntity, TDto> repository)
    : Controller, IControllerBase<TDto>
    where TEntity : class, IEntityBase
    where TDto : class, IDtoBase
{
    [HttpGet]
    public async Task<ActionResult<List<TDto>>> GetAll()
    {
        try
        {
            var response = new Response<List<TDto>>
            {
                Content = (await repository.GetAsync()).ToList(),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<TDto>>
            {
                Content = null,
                Error = new Error
                {
                    Code = "500",
                    Type = "Internal Server Error",
                    Message = environment.IsProduction()
                        ? "An error has occurred. Please retry later. If the problem persists, contact support."
                        : exception.Message
                }
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<Response<PagedResponse<TDto>>>> GetPage(QueryParams parameters)
    {
        try
        {
            var response = new Response<PagedResponse<TDto>>
            {
                Content = await repository.GetPageAsync(parameters),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<TDto>>
            {
                Content = null,
                Error = new Error
                {
                    Code = "500",
                    Type = "Internal Server Error",
                    Message = environment.IsProduction()
                        ? "An error has occurred. Please retry later. If the problem persists, contact support."
                        : exception.Message
                }
            });
        }
    }

    [HttpGet("id:long")]
    public async Task<ActionResult<Response<TDto>>> GetById(long id)
    {
        try
        {
            var response = new Response<TDto>
            {
                Content = await repository.GetByIdAsync(id),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<TDto>>
            {
                Content = null,
                Error = new Error
                {
                    Code = "500",
                    Type = "Internal Server Error",
                    Message = environment.IsProduction()
                        ? "An error has occurred. Please retry later. If the problem persists, contact support."
                        : exception.Message
                }
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Response<TDto>>> Create(TDto dto)
    {
        try
        {
            var response = new Response<TDto>
            {
                Content = await repository.CreateAsync(dto),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<TDto>>
            {
                Content = null,
                Error = new Error
                {
                    Code = "500",
                    Type = "Internal Server Error",
                    Message = environment.IsProduction()
                        ? "An error has occurred. Please retry later. If the problem persists, contact support."
                        : exception.Message
                }
            });
        }
    }

    [HttpPut]
    public async Task<ActionResult<Response<TDto>>> Update(TDto dto)
    {
        try
        {
            var response = new Response<TDto>
            {
                Content = await repository.UpdateAsync(dto),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<TDto>>
            {
                Content = null,
                Error = new Error
                {
                    Code = "500",
                    Type = "Internal Server Error",
                    Message = environment.IsProduction()
                        ? "An error has occurred. Please retry later. If the problem persists, contact support."
                        : exception.Message
                }
            });
        }
    }

    [HttpDelete("id:long")]
    public async Task<ActionResult<Response<bool>>> Delete(long id)
    {
        try
        {
            return Ok(new Response<bool>
            {
                Content = await repository.DeleteAsync(id)
            });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<bool?>
            {
                Content = null,
                Error = new Error
                {
                    Code = "500",
                    Type = "Internal server error",
                    Message = "An error has occurred please retry later"
                }
            });
        }
    }
}