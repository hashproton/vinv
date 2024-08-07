using System.ComponentModel.DataAnnotations;

namespace vinv.Entities;

public class Product
{
    public int Id { get; init; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public Category Category { get; set; } = null!;
    
    public int CategoryId { get; set; }
}
