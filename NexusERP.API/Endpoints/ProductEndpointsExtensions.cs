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
                foreach (var item in request.Metadata)
                {
                    product.UpdateMetadata(item.Key, item.Value);
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

        group.MapDelete("/{id:guid}", async (Guid id, IProductRepository repository) =>
        {
            var product = await repository.GetByIdAsync(id);
            if (product is null)
                return Results.NotFound();

            product.Deactivate();
            await repository.UpdateAsync(product);

            return Results.NoContent();
        });

        group.MapPatch("/{id:guid}/price", async (Guid id, UpdatePriceRequest request, IProductRepository repository) =>
        {
            var product = await repository.GetByIdAsync(id);
            if (product is null)
                return Results.NotFound();

            try
            {
                product.UpdatePrice(request.NewPrice);
                await repository.UpdateAsync(product);
                return Results.NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPatch("/{id:guid}/stock", async (Guid id, UpdateStockRequest request, IProductRepository repository) =>
        {
            var product = await repository.GetByIdAsync(id);
            if (product is null)
                return Results.NotFound();

            try
            {
                product.UpdateStock(request.NewStockQuantity);
                await repository.UpdateAsync(product);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, IProductRepository repository) =>
        {
            var product = await repository.GetByIdAsync(id);
            if (product is null)
                return Results.NotFound();
            
            if (!string.IsNullOrEmpty(request.NewName))
            {
                product.UpdateName(request.NewName);
            }

            if (request.NewMetadata != null)
            {
                foreach (var item in request.NewMetadata)
                {
                    product.UpdateMetadata(item.Key, item.Value);
                }
            }
            await repository.UpdateAsync(product);
            return Results.NoContent();
        });
    }
}
