using NexusERP.API.Domain;
using NexusERP.API.Data;
using NexusERP.API.DTOs;
using NexusERP.API.Domain.Interfaces;

namespace NexusERP.API.Endpoints;

public static class ProductEndpointsExtensions
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products");

        group.MapPost("/", async (CreateProductRequest request, IProductRepository repository) =>
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

            await repository.AddAsync(product);

            return Results.Created($"/products/{product.Id}", product);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProductRepository repository) =>
        {
            var product = await repository.GetByIdAsync(id);
            var dto = product is not null
                ? new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Price,
                    product.StockQuantity,
                    product.Metadata,
                    product.IsActive)
                : null;
            return dto is not null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
