namespace NexusERP.API.DTOs;

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int StockQuantity,
    Dictionary<string, string> Metadata,
    bool IsActive
);