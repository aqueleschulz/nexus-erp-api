using NexusERP.API.Domain;

namespace NexusERP.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_Should_ThrowException_When_PriceIsNegative()
    {
        var name = "Produto Teste";
        var invalidPrice = -10.00m; // "m" indica decimal
        var stock = 10;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
        {
            new Product(Guid.NewGuid(), name, invalidPrice, stock);
        });
        
        Assert.Contains("Price cannot be negative", exception.Message);
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_NameIsEmpty()
    {
        var emptyName = "";
        var price = 59.99m;
        var stock = 10;

        var exception = Assert.Throws<ArgumentException>(() => 
        {
            new Product(Guid.NewGuid(), emptyName, price, stock);
        });
        
        Assert.Contains("Product name cannot be empty", exception.Message);
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_StockIsNegative()
    {
        var name = "Produto Teste";
        var price = 59.99m;
        var invalidStock = -5;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
        {
            new Product(Guid.NewGuid(), name, price, invalidStock);
        });
        
        Assert.Contains("Stock cannot be negative", exception.Message);
    }

    [Fact]
    public void Deactivate_Should_Set_IsActiveToFalse()
    {
        var product = new Product(Guid.NewGuid(), "Gamer Chair", 1000m, 50);
        
        product.Deactivate();
        
        Assert.False(product.IsActive, "Product should be inactive");
        Assert.Equal(0, product.StockQuantity);
    }
}