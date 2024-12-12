using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Product { 


     public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Range(0.01, 10000.00)]
    public decimal BuyPrice { get; set; }  // Thêm thuộc tính BuyPrice

    [Range(0.01, 10000.00)]
    public decimal SellPrice { get; set; } // Thêm thuộc tính SellPrice

    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<ProductImage>? Images { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
}
