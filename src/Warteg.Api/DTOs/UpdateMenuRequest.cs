namespace Warteg.Api.DTOs;

public record UpdateMenuRequest(
    string Name,
    decimal Price,
    int Stock,
    int CategoryId
);