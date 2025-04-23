using GreenDonut.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Execution.Processing;
using HotChocolate.Types.Pagination;

namespace eShop.Catalog.Types;

[QueryType]
public static class ProductQueries
{
    [UsePaging]
    [UseFiltering<ProductsFilterInputType>]
    public static async Task<Connection<Product>> GetProductsAsync(
        PagingArguments pagingArguments,
        ProductService productService,
        IFilterContext filtering,
        ISelection selection,
        CancellationToken ct = default)
        => await productService.GetProductsAsync(
            pagingArguments,
            filtering.AsPredicate<Product>(), 
            selection.AsSelector<Product>(), 
            ct)
            .ToConnectionAsync();

    [NodeResolver]
    public static async Task<Product?> GetProductByIdAsync(
        int id,
        ProductService productService,
        CancellationToken cancellationToken)
        => await productService.GetProductByIdAsync(id, cancellationToken);
}
