using System.Linq.Expressions;
using eShop.Catalog.Services.Errors;
using HotChocolate.Data.Filters;
using HotChocolate.Execution.Processing;
using HotChocolate.Pagination;

namespace eShop.Catalog.Services;

public sealed class ProductService(
    CatalogContext context, 
    IProductByIdDataLoader productById,
    IProductsByBrandIdDataLoader productsByBrandId,
    IProductsByTypeIdDataLoader productsByTypeId)
{
    public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        => await productById.LoadAsync(id, cancellationToken);
    
    public async Task<Page<Product>> GetProductsAsync(
        PagingArguments pagingArguments,
        Expression<Func<Product, bool>>? predicate = null,
        Expression<Func<Product, Product>>? selector = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Products.AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (selector is not null)
        {
            query = query.Select(selector);
        }

        return await query.OrderBy(p => p.Id).ToPageAsync(pagingArguments, cancellationToken);
    }

    public async Task<Page<Product>?> GetProductsByBrandAsync(
        int brandId,
        CancellationToken ct = default)
        => await productsByBrandId.LoadAsync(brandId, ct);

    public async Task<Page<Product>?> GetProductsByTypeAsync(
        int typeId,
        CancellationToken ct = default)
        => await productsByTypeId.LoadAsync(typeId, ct);

    public async Task CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(product.Name);
        
        if (product.RestockThreshold >= product.MaxStockThreshold)
        {
            throw new MaxStockThresholdToSmallException(product.RestockThreshold, product.MaxStockThreshold);
        }
        
        if (!await context.Brands.AnyAsync(t => t.Id == product.BrandId, cancellationToken))
        {
            throw new EntityNotFoundException(nameof(Brand), product.BrandId);
        }
        
        if (!await context.ProductTypes.AnyAsync(t => t.Id == product.TypeId, cancellationToken))
        {
            throw new EntityNotFoundException(nameof(ProductType), product.TypeId);
        }
        
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        if (product.Id < 1)
        {
            throw new InvalidOperationException("Invalid product id.");
        }
        
        ArgumentException.ThrowIfNullOrEmpty(product.Name);
        
        if (product.RestockThreshold >= product.MaxStockThreshold)
        {
            throw new MaxStockThresholdToSmallException(product.RestockThreshold, product.MaxStockThreshold);
        }
        
        if (!await context.Brands.AnyAsync(t => t.Id == product.BrandId, cancellationToken))
        {
            throw new EntityNotFoundException(nameof(Brand), product.BrandId);
        }
        
        if (!await context.ProductTypes.AnyAsync(t => t.Id == product.TypeId, cancellationToken))
        {
            throw new EntityNotFoundException(nameof(ProductType), product.TypeId);
        }

        context.Products.Entry(product).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);
    }
}
