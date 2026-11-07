namespace KodaStore.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Warteg.Api.DTOs;
using Warteg.Api.Models;
using Warteg.Api.Services;


[ApiController]
[Route("api/v1/menu")]

public sealed class MenuController : ControllerBase
{
    private readonly IMenuServices _service;

    public MenuController(IMenuServices service)
    {
        _service = service;
    }

    [HttpGet("menu-category")]
    [ProducesResponseType(typeof(IReadOnlyList<MenuCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<MenuCategoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<MenuCategoryResponse>>> GetAllWithCategory(CancellationToken cancellationToken)
    {
        var menu = await _service.GetAllWithCategoryAsync(cancellationToken);
        return Ok(menu);
    }


    [HttpGet("{menuCode}")]
    [ProducesResponseType(typeof(MenuResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

    public async Task<ActionResult<MenuResponse>> GetByCode([FromRoute] string menuCode, CancellationToken cancellationToken)
    {
        var menu = await _service.GetByCodeAsync(menuCode, cancellationToken);
        return menu is null? Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak ditemukan"): Ok(menu);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MenuResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MenuResponse>> Create([FromBody] MenuRequest request, CancellationToken cancellationToken)
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
                new {menuCode = created.MenuCode},
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

    [HttpDelete("{menuCode}")]
    [ProducesResponseType(typeof(MenuResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string menuCode, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(menuCode, cancellationToken);
        return deleted ? NoContent() : Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Gagal delete",
            detail: $"gagal {menuCode}"
        );
    }

    [HttpPut("{menuCode}")]
    [ProducesResponseType(typeof(MenuResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuResponse>> Update([FromBody] UpdateMenuRequest request, [FromRoute] string menuCode, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateAsync(menuCode, request, cancellationToken);
            return updated is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "gagal update", detail: $"No product was found for code {menuCode}") : Ok(updated);
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