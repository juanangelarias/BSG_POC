using BSG.Api.Controllers.Base;
using BSG.Common.DTO;
using BSG.Entities;
using BSG.Repository;
using BSG.Repository.Base;
using Microsoft.AspNetCore.Mvc;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComponentController(IWebHostEnvironment environment, IComponentRepository repository)
    : ControllerBase<Component, ComponentDto>(environment, repository)
{
}