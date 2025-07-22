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
    void CheckMissingProductType();
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
        if(product.ProductType == null)
            return;
        
        product.ProductTypeId = product.ProductType?.Id ?? 0;
        product.ProductType = null;
        await productService.Create(product);
        
        await Get();
    }

    public async Task CreateMany(List<ProductDto> products)
    {
        products.ForEach(f => f.ProductType = null);
        await productService.CreateMany(products);
        
        await Get();
    }

    public async Task Update(ProductDto product)
    {
        product.ProductType = null;
        await productService.Update(product);
        
        await Get();
    }

    public async Task UpdateMany(List<ProductDto> products)
    {
        var toSubmit = products.Select(s=> s.GetCopy()).ToList();
        toSubmit.ForEach(f => f.ProductType = null);
        
        await productService.UpdateMany(toSubmit);

        await Get();
    }

    public async Task Delete(long productId)
    {
        await productService.Delete(productId);
        
        await Get();
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

    public void CheckMissingProductType()
    {
        foreach (var prd in Products.Where(r=>r.ProductType == null))
        {
            prd.ProductType = ProductTypes.FirstOrDefault(f => f.Id == prd.ProductTypeId);
        }
    }

    private ProductDto GetCopy(ProductDto input)
    {
        var output = input.GetCopy();
        
        return output;
    }
}