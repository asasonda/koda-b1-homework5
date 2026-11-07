using Warteg.Api.DTOs;
using Warteg.Api.Models;

namespace Warteg.Api.Repositories;

public interface IMenuRepository
{
    Task<IReadOnlyList<MenuCategoryResponse>> GetAllWithCategoryAsync(CancellationToken cancellationToken);
    Task<Menu?> GetByIdAsync(string menuCode, CancellationToken cancellationToken);
    Task<bool> AddMenuAsync(Menu menu, CancellationToken cancellationToken);
    Task<bool> UpdateMenuAsync(Menu menu, CancellationToken cancellationToken);
    Task<bool> DeleteMenuAsync(string menuCode, CancellationToken cancellationToken);
}