using BSG.Api.Controllers.Base;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Entities;
using BSG.Repository;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Error = BSG.Common.Model.Error;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IWebHostEnvironment environment, IProductRepository repository) 
    : ControllerBase<Product,ProductDto>(environment, repository)
{
    private readonly IWebHostEnvironment _environment = environment;

    [HttpGet("getExtended")]
    public async Task<ActionResult<List<ProductDto>>> GetExtended()
    {
        try
        {
            var response = new Response<List<ProductDto>>
            {
                Content = (await repository.GetExtended()).ToList(),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<ProductDto>>
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

    [HttpGet("search/{argument}")]
    public async Task<ActionResult<Response<List<ProductDto>>>> Search([FromRoute] string argument)
    {
        try
        {
            var response = new Response<List<ProductDto>>
            {
                Content = (await repository.Search(argument)).ToList(),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<ProductDto>>
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

    [HttpPost("createMany")]
    public async Task<ActionResult<Response<List<ProductDto>>>> CreateMany([FromBody] List<ProductDto> products)
    {
        try
        {
            var response = new Response<List<ProductDto>>
            {
                Content = (await repository.CreateManyAsync(products)).ToList(),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<ProductDto>>
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

    [HttpPost("seed")]
    public async Task<ActionResult<Response<bool>>> Seed()
    {
        try
        {
            await repository.SeedProducts();
            var response = new Response<bool>
            {
                Content = true,
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<ProductDto>>
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

    [HttpPut("updateMany")]
    public async Task<IActionResult> UpdateMany([FromBody] List<ProductDto> products)
    {
        try
        {
            var response = new Response<List<ProductDto>>
            {
                Content = (await repository.UpdateManyAsync(products)).ToList(),
                Error = null
            };

            return Ok(response);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<List<ProductDto>>
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