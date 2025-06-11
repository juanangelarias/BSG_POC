using BSG.Api.Controllers.Base;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Entities;
using BSG.Repository;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ElementController(IWebHostEnvironment environment, IElementRepository repository, IElementRepository elementRepository)
    : ControllerBase<Element, ElementDto>(environment, repository)
{
    private readonly IWebHostEnvironment _environment = environment;
    
    [HttpGet("getByComponentId/{componentId:long}")]
    public async Task<ActionResult<List<ElementDto>>> GetByComponentIdAsync(long componentId)
    {
        try
        {
            var response = new Response<List<ElementDto>>
            {
                Content = (await elementRepository.GetByComponentIdAsync(componentId)).ToList(),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<ElementDto>>
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