using BSG.Api.Controllers.Base;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Entities;
using BSG.Features;
using BSG.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IWebHostEnvironment environment, IUserRepository repository, IUserFeature feature) 
    : ControllerBase<User, UserDto>(environment, repository)
{
    [HttpGet("getByUsername/{username:alpha}")]
    public async Task<ActionResult<Response<UserDto>>> Get([FromRoute] string username)
    {
        try
        {
            return Ok(new Response<UserDto>
            {
                Content = await repository.GetByUsername(username)
            });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<UserDto>
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
    
    [AllowAnonymous]
    [HttpGet("getByUsernameAndEmailToken/{username}/{token}")]
    public async Task<ActionResult<Response<UserDto>>> GetByUsernameAndEmailToken([FromRoute] string username,
        [FromRoute] string token)
    {
        try
        {
            var user = await repository.GetByUsernameAndEmailToken(username, token);
            if (user == null)
                return Ok(new Response<UserDto>
                {
                    Content = null,
                    Error = new Error
                    {
                        Code = "400",
                        Type = "Not Found",
                        Message = "User not found. Or toke is invalid or expired"
                    }
                });
            return Ok(new Response<UserDto>
            {
                Content = user
            });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<UserDto>
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

    [HttpGet("{userId:long}/GetUser")]
    public async Task<ActionResult<Response<UserDto>>> GetUser([FromRoute] long userId)
    {
        try
        {
            var user  = await feature.GetUser(userId);

            return Ok(new Response<UserDto> { Content = user, Error = null, ExecutionTime = DateTime.Now });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<UserDto>
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

    [HttpGet("{userId:long}/GetMetadata")]
    public async Task<ActionResult<Response<List<Metadata>>>> GetMetadata([FromRoute] long userId)
    {
        try
        {
            var metadata = await feature.GetMetadata(userId);

            return Ok(new Response<List<Metadata>> { Content = metadata, Error = null, ExecutionTime = DateTime.Now });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<UserDto>
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
    
    [HttpPost("{userId:long}/sendWelcomeEmail")]
    public async Task<IActionResult> SendWelcomeEmail([FromRoute] long userId)
    {
        await feature.SendWelcomeEmail(userId);
        return NoContent();
    }
    
    [AllowAnonymous]
    [HttpPost("sendForgotPasswordEmail")]
    public async Task<IActionResult> SendForgotPasswordEmail([FromQuery] string username)
    {
        await feature.SendForgotPasswordEmail(username);
        return NoContent();
    }

    [HttpPost("{userId:long}/sendPasswordChangedConfirmationEmail")]
    public async Task<IActionResult> SendPasswordChangedConfirmation([FromRoute] long userId)
    {
        await feature.SendPasswordChangedConfirmationEmail(userId);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ActionResult<Response<LoginResponse>>> Authenticate([FromBody] LoginRequest request)
    {
        try
        {
            var response = await feature.Login(request);
            if (response.User == null)
                return Ok(new Response<LoginResponse>
                {
                    Content = null,
                    Error = new Error
                    {
                        Code = "400",
                        Type = "Bad request",
                        Message = "Invalid username or password"
                    }
                });
            return Ok(new Response<LoginResponse>
            {
                Content = response
            });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "{Message}", exception.Message);
            return Ok(new Response<LoginResponse>
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

    [HttpPost("{userId:long}/verifyEmailExistence")]
    public async Task<ActionResult<Response<bool?>>> ValidateEmail([FromRoute] long userId, [FromBody] string email)
    {
        try
        {
            return Ok(new Response<bool>
            {
                Content = await repository.VerifyEmailExistence(userId, email)
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