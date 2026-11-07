using Warteg.Api.Models;

namespace Warteg.Api.DTOs;

public sealed record class MenuResponse(string MenuCode, string Name, decimal Price, int Stock, int CategoryId)
{
    public static MenuResponse FromModel(Menu menu) => new(menu.MenuCode, menu.Name, menu.Price, menu.Stock, menu.CategoryId);
}