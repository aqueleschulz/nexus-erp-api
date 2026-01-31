namespace NexusERP.API.DTOs;

public record CreateProductRequest(
    string Name,
    decimal Price,
    int StockQuantity,
    Dictionary<string, string>? Metadata
);