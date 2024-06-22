using System.ComponentModel.DataAnnotations;

namespace DotK_TechShop.Models.Db;

public class Product
{
    [Key]
    public int ProductID { get; set; }


    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;


    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    [StringLength(455)]
    public string? Image { get; set; } = string.Empty;


    [Required]
    [Range(0, 1000)]
    public int? RAM { get; set; }

    [Required]
    [Range(0, 10240)]
    public int? Storage { get; set; }


    // Forein Key
    public int BrandID { get; set; }


    // References
    public Brand? Brand { get; set; }
    public ICollection<Discount>? Discounts { get; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}
