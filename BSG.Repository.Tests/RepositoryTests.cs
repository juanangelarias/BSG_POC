using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace BSG.Repository.Tests;

[TestFixture]
public class YourRepositoryTests
{
    private IComponentRepository _repository;

    public static List<Component> _components =
    [
        new()
        {
            Id = 1,
            Name = "Component 1",
            Description = "Description of the component 1",
            CreatedById = -1,
            CreatedOn = DateTime.Now,
            ModifiedById = -1,
            ModifiedOn = DateTime.Now,
        },

        new()
        {
            Id = 2,
            Name = "Component 2",
            Description = "Description of the component 2",
            CreatedById = -1,
            CreatedOn = DateTime.Now,
            ModifiedById = -1,
            ModifiedOn = DateTime.Now,
        },

        new()
        {
            Id = 3,
            Name = "Component 3",
            Description = "Description of the component 3",
            CreatedById = -1,
            CreatedOn = DateTime.Now,
            ModifiedById = -1,
            ModifiedOn = DateTime.Now,
        }
    ];

    private Mock<BsgDbContext> _db;
    private Mock<IMapper> _mapper;

    [SetUp]
    public void Setup()
    {
        // Initialize repository with test configuration or mock dependencies
        _mapper = new Mock<IMapper>();

        _db = new Mock<BsgDbContext>();
        _db.Setup<DbSet<Component>>(x => x.Components)
            .ReturnsDbSet(_components);
        
        _repository = new ComponentRepository(_mapper.Object, _db.Object);
    }

    [Test]
    public async Task ShouldGet3Items()
    {
        var components = await _repository.GetAsync();
        
        Assert.Equals(3, components.Count());
    }

    [Test]
    public async Task ShouldComponentNameEqualToComponent1()
    {
        var component = await _repository.GetByIdAsync(1);

        Assert.Equals("Component 1", component.Name);
    }

    [Test]
    public async Task Create()
    {
        var newComponent = await _repository.CreateAsync(new ComponentDto
        {
            Id = 0,
            Name = "Component 1000",
            Description = "Description of Component 1000",
        });
        
        Assert.Equals(4, newComponent.Id);

        var components = (await _repository.GetAsync()).ToList();
        Assert.Equals(4, components.Count);
        
        var checkComponent = components.FirstOrDefault(x => x.Id == 4);
        Assert.Equals("Component 1000", checkComponent?.Name ?? "");
    }
}
