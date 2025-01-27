using HotChocolate.Pagination;

namespace eShop.Catalog.Services;

internal static class ProductDataLoader
{
    [DataLoader]
    public static async Task<Dictionary<int, Product>> GetProductByIdAsync(
        IReadOnlyList<int> ids,
        CatalogContext context,
        CancellationToken ct)
        => await context.Products
            .AsNoTracking()
            .Where(t => ids.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, ct);

    [DataLoader]
    public static async Task<Dictionary<int, Page<Product>>> GetProductsByBrandIdAsync(
        IReadOnlyList<int> keys,
        PagingArguments args,
        CatalogContext context,
        CancellationToken ct)
    => await context.Products.Where(p => keys.Contains(p.BrandId)).OrderBy(p => p.Id).ToBatchPageAsync(p => p.Id, args, ct);
    
    [DataLoader]
    public static async Task<Dictionary<int, Page<Product>>> GetProductsByTypeIdAsync(
        IReadOnlyList<int> keys,
        PagingArguments args,
        CatalogContext context,
        CancellationToken ct) =>
    await context.Products.Where(p => keys.Contains(p.Id)).OrderBy(p => p.Id).ToBatchPageAsync(p => p.Id, args, ct);
}
