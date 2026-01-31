using NexusERP.API.Domain;
using NexusERP.API.Data;
using NexusERP.API.DTOs;

namespace NexusERP.API.Endpoints;

public static class ProductEndpointsExtensions
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products");

        group.MapPost("/", async (CreateProductRequest request, AppDbContext dbContext) =>
        {
            if (string.IsNullOrEmpty(request.Name))
                return Results.BadRequest("Product name is required.");

            var product = new Product(
                Guid.NewGuid(),
                request.Name,
                request.Price,
                request.StockQuantity);

            if (request.Metadata != null)
            {
                foreach (var kvp in request.Metadata)
                {
                    product.Metadata[kvp.Key] = kvp.Value;
                }
            }

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            //201 Created
            return Results.Created($"/products/{product.Id}", product);
        });
    }
}
