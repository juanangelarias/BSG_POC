using BSG.Api.Controllers.Base;
using BSG.Common.DTO.Base;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.AspNetCore.Mvc;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductTypeController(IWebHostEnvironment environment, IRepositoryExtended<ProductType, ProductTypeDto> repository) 
    : ControllerBase<ProductType, ProductTypeDto>(environment, repository)
{
}