using Warteg.Api.Models;

namespace Warteg.Api.DTOs;

public sealed record CategoryResponse(string CategoryCode, string Name)
{
    public static CategoryResponse FromModel(Category category) => new(category.CategoryCode, category.Name);
}