using Warteg.Api.DTOs;
using Warteg.Api.Models;
using Warteg.Api.Repositories;

namespace Warteg.Api.Services;

public sealed class MenuServices : IMenuServices
{
    private readonly IMenuRepository _repository;

    public MenuServices(IMenuRepository repository)
    {
        _repository = repository;
    }

    public async Task<MenuResponse?> CreateAsync(MenuRequest request, CancellationToken cancellationToken)
    {
        var menu = new Menu(request.MenuCode, request.Name, request.Price, request.Stock, request.CategoryId);
        var created = await _repository.AddMenuAsync(menu, cancellationToken);

        return created ? MenuResponse.FromModel(menu) : null;
    }

    public Task<bool> DeleteAsync(string menuCode, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(menuCode) || menuCode.Trim().Length > 30)
        {
            return Task.FromResult(false);
        }
        return _repository.DeleteMenuAsync(menuCode, cancellationToken);
    }

    public Task<IReadOnlyList<MenuCategoryResponse>> GetAllWithCategoryAsync(CancellationToken cancellationToken)
    {
        return _repository.GetAllWithCategoryAsync(cancellationToken);
    }

    public async Task<MenuResponse?> GetByCodeAsync(string menuCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(menuCode) || menuCode.Trim().Length > 30)
        {
            return null;
        }
        var menu = await _repository.GetByIdAsync(menuCode, cancellationToken);
        return menu is null ? null : MenuResponse.FromModel(menu);
    }

    public async Task<MenuResponse?> UpdateAsync(string menuCode, UpdateMenuRequest request, CancellationToken cancellationToken)
    {
        var menu = new Menu(menuCode, request.Name, request.Price, request.Stock, request.CategoryId);
        var updated = await _repository.UpdateMenuAsync(menu, cancellationToken);
        return updated ? MenuResponse.FromModel(menu) : null;
    }
}