using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Entities;
using BSG.Repository;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Error = BSG.Common.Model.Error;

namespace BSG.Api.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public class NotificationController(
    IWebHostEnvironment environment,
    INotificationRepository repository)
    : ControllerBase<Notification, NotificationDto>(environment, repository)
{
    [HttpGet("GetByEmail/{email:alpha}")]
    public async Task<ActionResult<List<NotificationDto>>> GetNotificationByEmail(string email)
    {
        try
        {
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
                    Message = environment.IsProduction()
                        ? "An error has occurred. Please retry later. If the problem persists, contact support."
                        : exception.Message
                }
            });
        }
    } 
}