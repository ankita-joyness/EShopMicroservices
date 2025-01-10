using Marten.Linq.QueryHandlers;

using System.Data;

namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryQueryHandler
        (IDocumentSession session) 
        : IQueryHandler<GetProductByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        
        var products = await session.Query<Product>().Where(p => p.Category.Contains(query.Category)).ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}
