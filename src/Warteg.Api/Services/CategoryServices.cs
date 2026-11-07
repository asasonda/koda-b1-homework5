using Warteg.Api.DTOs;
using Warteg.Api.Models;
using Warteg.Api.Repositories;

namespace Warteg.Api.Services;

public sealed class CategoryServices : ICategoryServices
{
    private readonly ICategoryRepository _repository;

    public CategoryServices(ICategoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<CategoryResponse?> CreateAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category(request.CategoryCode, request.Name);
        var created = await _repository.AddCategoryAsync(category, cancellationToken);
        return created ? CategoryResponse.FromModel(category) : null;
    }

    public Task<bool> DeleteAsync(string categoryCode, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(categoryCode) || categoryCode.Trim().Length > 50)
        {
            return Task.FromResult(false);
        }
        return _repository.DeleteCategoryAsync(categoryCode, cancellationToken);
    }

    public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(cancellationToken);
        return products.Select(CategoryResponse.FromModel).ToArray();
    }

    public async Task<CategoryResponse?> GetByCodeAsync(string categoryCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(categoryCode) || categoryCode.Trim().Length > 30)
        {
            return null;
        }
        var category = await _repository.GetByIdAsync(categoryCode, cancellationToken);
        return category is null ? null : CategoryResponse.FromModel(category);
    }

    public async Task<CategoryResponse?> UpdateAsync(string categoryCode, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category(categoryCode, request.Name);
        var updated = await _repository.UpdateCategoryAsync(category, cancellationToken);
        return updated ? CategoryResponse.FromModel(category) : null;
    }
}