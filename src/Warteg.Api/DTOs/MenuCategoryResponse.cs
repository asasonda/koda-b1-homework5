namespace Warteg.Api.DTOs;

public record MenuCategoryResponse(
    string MenuCode,
    string Name,
    decimal Price,
    int Stock,
    string CategoryName
);