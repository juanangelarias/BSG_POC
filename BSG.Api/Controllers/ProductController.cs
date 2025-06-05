using BSG.Api.Controllers.Base;
using BSG.Common.DTO;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.AspNetCore.Mvc;

namespace BSG.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IWebHostEnvironment environment, IRepositoryExtended<Product, ProductDto> repository) 
    : ControllerBase<Product,ProductDto>(environment, repository)
{
}