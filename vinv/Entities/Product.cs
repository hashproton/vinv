namespace vinv.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Category Category { get; set; }
    
    public int CategoryId { get; set; }
}
