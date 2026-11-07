using Warteg.Api.DTOs;

namespace Warteg.Api.Services;

public interface ICategoryServices
{
    Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<CategoryResponse?> GetByCodeAsync(string categoryCode, CancellationToken cancellationToken);
    Task<CategoryResponse?> CreateAsync(CategoryRequest request, CancellationToken cancellationToken);
    Task<CategoryResponse?> UpdateAsync(string categoryCode, UpdateCategoryRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string categoryCode, CancellationToken cancellationToken);
}