using BSG.Api.Controllers.Base;
using BSG.Common.DTO;
using BSG.Entities;
using BSG.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationPropertyController(
    IWebHostEnvironment environment,
    INotificationPropertyDefinitionRepository repository)
    : ControllerBase<NotificationPropertyDefinition, NotificationPropertyDefinitionDto>(environment, repository)
{
}