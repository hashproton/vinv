using System.ComponentModel.DataAnnotations;

namespace vinv.Entities;

public class Category
{
    [Key]
    public int Id { get; init; }

    [StringLength(100)]
    public string Name { get; init; } = null!;

    public List<Product> Products { get; init; } = [];
}