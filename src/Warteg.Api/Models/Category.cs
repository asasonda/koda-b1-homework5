
namespace Warteg.Api.Models;

public sealed class Category
{
    public string CategoryCode {get; }
    public string Name {get; }

    public Category(string categoryCode, string name)
    {
        if (string.IsNullOrWhiteSpace(categoryCode))
        {
            throw new ArgumentException("CategoryCode not valid", nameof(categoryCode));
        }

        if (categoryCode.Length > 50)
        {
            throw new ArgumentException("CategoryCode maximum 50 characters", nameof(categoryCode));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name not valid", nameof(name));
        }

        if (name.Length > 100)
        {
            throw new ArgumentException("Name maximum 100 characters", nameof(name));
        }

        CategoryCode = categoryCode;
        Name = name;
    }
}
