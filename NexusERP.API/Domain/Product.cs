
namespace NexusERP.API.Domain;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; } = new Dictionary<string, string>();
    public bool IsActive { get; private set; } = true;

    //protected Product() { }

    public Product(Guid id, string name, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");

        Id = id;
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void Deactivate()
    {
        StockQuantity = 0;
        IsActive = false;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(newName));
        }
        Name = newName;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price cannot be negative.");
        }
        Price = newPrice;
    }

    public void UpdateStock(int quantityChange)
    {
        int newQuantity = quantityChange;
        if (newQuantity < 0)
        {
            throw new InvalidOperationException("Stock quantity cannot be negative.");
        }
        StockQuantity = newQuantity;
    }

    public void UpdateMetadata(string key, string value)
    {
        if(Metadata.ContainsKey(key))
            Metadata[key] = value;
        else
            Metadata.Add(key, value);
    }
}