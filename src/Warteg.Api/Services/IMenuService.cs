using Warteg.Api.DTOs;

namespace Warteg.Api.Services;

public interface IMenuServices
{
    Task<IReadOnlyList<MenuCategoryResponse>> GetAllWithCategoryAsync(CancellationToken cancellationToken);
    Task<MenuResponse?> GetByCodeAsync(string menuCode, CancellationToken cancellationToken);
    Task<MenuResponse?> CreateAsync(MenuRequest request, CancellationToken cancellationToken);
    Task<MenuResponse?> UpdateAsync(string menuCode, UpdateMenuRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string menuCode, CancellationToken cancellationToken);
}