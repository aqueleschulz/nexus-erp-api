namespace NexusERP.API.DTOs;

public record UpdateProductRequest(
    string NewName,
    Dictionary<string, string>? NewMetadata
);