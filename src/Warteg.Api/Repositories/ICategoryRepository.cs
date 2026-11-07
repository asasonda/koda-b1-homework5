using Warteg.Api.Models;

namespace Warteg.Api.Repositories;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(string categoryCode, CancellationToken cancellationToken);
    Task<bool> AddCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<bool> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<bool> DeleteCategoryAsync(string categoryCode, CancellationToken cancellationToken);
}