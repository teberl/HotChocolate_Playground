using GreenDonut.Data;
using HotChocolate.Types.Pagination;

namespace eShop.Catalog.Types;

[ObjectType<ProductType>]
public static partial class ProductTypeNode
{
    [UsePaging]
    public static Task<Connection<Product>> GetProductsAsync(
        [Parent] ProductType productType,
        ProductService productService,
        CancellationToken cancellationToken)
        => productService.GetProductsByTypeAsync(productType.Id, cancellationToken).ToConnectionAsync();
}