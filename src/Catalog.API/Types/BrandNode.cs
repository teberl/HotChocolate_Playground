using HotChocolate.Pagination;
using HotChocolate.Types.Pagination;

namespace eShop.Catalog.Types;

[ObjectType<Brand>]
public static partial class BrandNode
{
    static partial void Configure(IObjectTypeDescriptor<Brand> descriptor)
    {
        descriptor.Ignore(t => t.Subscriptions);
    }

    [UsePaging]
    public static async Task<Connection<Product>> GetProductsAsync(
        [Parent] Brand brand,
        ProductService productService,
        CancellationToken cancellationToken)
        => await productService.GetProductsByBrandAsync(brand.Id, cancellationToken).ToConnectionAsync();
}