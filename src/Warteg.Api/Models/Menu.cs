namespace Warteg.Api.Models;

public sealed class Menu{
    public string MenuCode {get; }
    public string Name {get;}
    public decimal Price {get;}
    public int Stock {get;}
    public int CategoryId {get;}

    public Menu(string menuCode, string name, decimal price, int stock, int categoryId)
    {
        if (menuCode.Length > 50)
        {
            throw new ArgumentException("MenuCode not valid", nameof(menuCode));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name not valid", nameof(name));
        }
        if(price <= 0)
        {
            throw new ArgumentOutOfRangeException("Price cannot be negative", nameof(price));
        }
        if(stock < 0)
        {
            throw new ArgumentOutOfRangeException("Stock cannot be negative", nameof(stock));
        }
        if(categoryId < 0)
        {
            throw new ArgumentOutOfRangeException("Id cannot be negative", nameof(categoryId));
        }

        MenuCode = menuCode;
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }
}