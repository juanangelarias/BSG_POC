using BSG.Api.Controllers.Base;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Entities;
using BSG.Repository;
using BSG.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComponentController(IWebHostEnvironment environment, IComponentRepository repository)
    : ControllerBase<Component, ComponentDto>(environment, repository)
{
    private readonly IWebHostEnvironment _environment = environment;

    [HttpGet("GetExtended")]
    public async Task<ActionResult<List<ComponentDto>>> GetExtended()
    {
        try
        {
            var response = new Response<List<ComponentDto>>
            {
                Content = await repository.GetExtended(),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<ComponentDto>>
            {
                Content = null,
                Error = new Error
                {
                    Code = "500",
                    Type = "Internal Server Error",
                    Message = _environment.IsProduction()
                        ? "An error has occurred. Please retry later. If the problem persists, contact support."
                        : exception.Message
                }
            });
        }
    }
    
}