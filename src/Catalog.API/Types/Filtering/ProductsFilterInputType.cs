
using HotChocolate.Data.Filters;

namespace eShop.Catalog.Types.Filtering;

public class ProductsFilterInputType : FilterInputType<Product>
{
    protected override void Configure(IFilterInputTypeDescriptor<Product> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(filterEntity => filterEntity.Name).Type<StringOperationFilterInputType>();
        descriptor.Field(filterEntity => filterEntity.BrandId).Type<IdOperationFilterInputType>();
        descriptor.Field(filterEntity => filterEntity.ArrivalDate).Type<DateTimeOperationFilterInputType>();
    }
}