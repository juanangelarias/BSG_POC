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
public class NotificationController(
    IWebHostEnvironment environment,
    INotificationRepository repository)
    : ControllerBase<Notification, NotificationDto>(environment, repository)
{
    private readonly IWebHostEnvironment _environment = environment;
    
    [HttpGet("GetByEmail/{user:alpha}/{server:alpha}/{ending:alpha}")]
    public async Task<ActionResult<List<NotificationDto>>> GetNotificationByEmail(string user, string server, string ending)
    {
        try
        {
            var email = $"{user}@{server}.{ending}";

            var response = new Response<List<NotificationDto>>
            {
                Content = (await repository.GetByEmail(email)).ToList(),
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