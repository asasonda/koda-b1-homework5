namespace KodaStore.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Warteg.Api.DTOs;
using Warteg.Api.Models;
using Warteg.Api.Services;


[ApiController]
[Route("api/v1/category")]

public sealed class CategoryController : ControllerBase
{
    private readonly ICategoryServices _service;

    public CategoryController(ICategoryServices service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<Category>>> GetAll(CancellationToken cancellationToken)
    {
        var category = await _service.GetAllAsync(cancellationToken);
        return Ok(category);
    }


    [HttpGet("{categoryCode}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

    public async Task<ActionResult<CategoryResponse>> GetByCode([FromRoute] string categoryCode, CancellationToken cancellationToken)
    {
        var category = await _service.GetByCodeAsync(categoryCode, cancellationToken);
        return category is null? Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak ditemukan"): Ok(category);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(request, cancellationToken);
            if(created is null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak dapat di dibuat");
            }

            return CreatedAtAction(
                nameof(GetByCode),
                new {categoryCode = created.CategoryCode},
                created
            );
        }
        catch (System.Exception e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "invalid create data",
                detail: e.Message
            );
        }
    }

    [HttpDelete("{categoryCode}")]
    [ProducesResponseType(typeof(CategoryResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string categoryCode, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(categoryCode, cancellationToken);
        return deleted ? NoContent() : Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Gagal delete",
            detail: $"gagal {categoryCode}"
        );
    }

    [HttpPut("{categoryCode}")]
    [ProducesResponseType(typeof(CategoryResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryResponse>> Update([FromBody] UpdateCategoryRequest request, [FromRoute] string categoryCode, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateAsync(categoryCode, request, cancellationToken);
            return updated is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "gagal update", detail: $"No product was found for code {categoryCode}") : Ok(updated);
        }
        catch (System.Exception e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "invalid update data",
                detail: e.Message
            );
        }
    }

}