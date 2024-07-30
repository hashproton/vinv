namespace vinv.Entities;

public class ProductStock
{
    public int Id { get; set; }
    
    public decimal Stock { get; set; }

    public decimal MinimalStock { get; set; }
    
    public Product Product { get; set; }
    
    public int ProductId { get; set; }

    public bool IsAvailable => Stock > MinimalStock;
    
    public bool IsLowStock => Stock <= MinimalStock;
}