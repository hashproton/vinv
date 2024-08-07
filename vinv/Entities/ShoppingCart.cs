namespace vinv.Entities;

public class ShoppingCart
{
    public int Id { get; set; }

    public List<ProductStock> ProductStocks { get; set; } = [];
}