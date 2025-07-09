using BSG.Common.DTO;
using BSG.DataServices;
using BSG.States.Base;

namespace BSG.App.Product.State;

public interface IProductState
{
    List<ProductDto> Products { get; set; }
    List<ProductTypeDto> ProductTypes { get; set; }

    Task Create(ProductDto product);
    Task CreateMany(List<ProductDto> products);
    Task Update(ProductDto product);
    Task UpdateMany(List<ProductDto> products);
    Task Delete(long productId);
    Task Get();
}

public class ProductState(IProductDataService productService, IProductTypeDataService productTypeService)
    : StateBase, IProductState
{
    #region Fields & Properties

    #region Products

    private List<ProductDto> _products = [];

    public List<ProductDto> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ProductTypes

    private List<ProductTypeDto> _productTypes = [];

    public List<ProductTypeDto> ProductTypes
    {
        get => _productTypes;
        set
        {
            _productTypes = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    public async Task Create(ProductDto product)
    {
        product.ProductType = null;
        await productService.Create(product);
    }

    public async Task CreateMany(List<ProductDto> products)
    {
        products.ForEach(f => f.ProductType = null);
        await productService.CreateMany(products);
    }

    public async Task Update(ProductDto product)
    {
        product.ProductType = null;
        await productService.Update(product);
    }

    public async Task UpdateMany(List<ProductDto> products)
    {
        products.ForEach(f => f.ProductType = null);
        await productService.UpdateMany(products);
    }

    public async Task Delete(long productId)
    {
        await productService.Delete(productId);
    }

    public async Task Get()
    {
        var tasks = new List<Task>
        {
            GetProducts(),
            GetProductTypes()
        };

        await Task.WhenAll(tasks);
    }

    private async Task GetProducts()
    {
        Products = await productService.GetExtended();
    }

    private async Task GetProductTypes()
    {
        ProductTypes = (await productTypeService.Get())
            .OrderBy(o=>o.Name)
            .ToList();   
    }
}